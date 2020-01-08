using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Festispec.DomainServices.Factories;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.Questions;
using Festispec.UI.Interfaces;
using Festispec.UI.Views.Controls;
using GalaSoft.MvvmLight.Command;
using IronPdf;

namespace Festispec.UI.ViewModels
{
    public class RapportPreviewViewModel : BaseValidationViewModel
    {
        private readonly IFestivalService _festivalService;
        private readonly IFrameNavigationService _navigationService;
        private readonly IQuestionnaireService _questionnaireService;
        private readonly GraphSelectorFactory _graphFactory;
        private string _pdfHtml;


        private readonly Dictionary<Image, string> imageSources = new Dictionary<Image, string>();


        public RapportPreviewViewModel(
            IFrameNavigationService navigationService,
            IQuestionnaireService questionnaireService,
            IFestivalService festivalService,
            GraphSelectorFactory graphSelector
            )
        {
            _festivalService = festivalService;
            _questionnaireService = questionnaireService;
            _navigationService = navigationService;
            _graphFactory = graphSelector;

            GeneratePdfCommand = new RelayCommand(SavePdf);
            BackCommand = new RelayCommand(Back);

            SelectedFestival = _festivalService.GetFestival((int)_navigationService.Parameter);

            if (SelectedFestival.Questionnaires.Count == 0)
                _navigationService.NavigateTo("FestivalInfo", _navigationService.Parameter);

            GenerateReport();
        }

        public ObservableCollection<FrameworkElement> Controls { get; set; }
        public Festival SelectedFestival { get; set; }
        public string DescriptionText { get; set; }


        public ICommand GeneratePdfCommand { get; set; }
        public ICommand BackCommand { get; set; }


        private void CreateReport()
        {
            _pdfHtml = "";
            _pdfHtml += string.Format("<h1>Rapport {0}</h1>", SelectedFestival.FestivalName);
        }

        private void GenerateReport()
        {
            int questionaireId = SelectedFestival.Questionnaires.FirstOrDefault().Id;
            List<Question> questions = _questionnaireService.GetQuestionsFromQuestionnaire(questionaireId);

            Controls = new ObservableCollection<FrameworkElement>();

            CreateReport();

            Controls.Add(CreateLabel("Beschrijving"));
            Controls.Add(new TextBox
            {
                Height = 150,
                Width = 700,
                Margin = new Thickness(10),
                AcceptsReturn = true,
                AcceptsTab = true,
                TextWrapping = TextWrapping.Wrap
            });

            foreach (Question question in questions)
                AddQuestionToReport(question);
        }

        private void Back()
        {
            _navigationService.NavigateTo("FestivalInfo", SelectedFestival.Id);
        }

        private void AddQuestionToReport(Question question)
        {
            // Make sure that the question point to its reference when available.
            if (question is ReferenceQuestion)
                question = ((ReferenceQuestion)question).Question;

            var converter = _graphFactory.GetConverter(question.GraphType);

            if (converter == null && !(question is UploadPictureQuestion || question is StringQuestion))
                return;

            if (question is UploadPictureQuestion || question is StringQuestion)
            {
                Controls.Add(CreateLabel(question.Contents));
                AddAnswers(question.Answers);

                return;
            }

            List<GraphableSeries> chartValues = converter.TypeToChart(question);

            if (chartValues.Count < 1)
                return;

            var lineControl = new Control();


            if (question.GraphType == GraphType.Line)
                lineControl = new LineChartControl(chartValues);
            if (question.GraphType == GraphType.Pie)
                lineControl = new PieChartControl(chartValues);
            else if (question.GraphType == GraphType.Column)
                lineControl = new ColumnChartControl(chartValues);

            lineControl.Height = 300;
            lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;

            Controls.Add(CreateLabel(question.Contents));
            Controls.Add(lineControl);
            Controls.Add(new TextBox
            {
                Height = 150,
                Width = 700,
                Margin = new Thickness(10),
                AcceptsReturn = true,
                AcceptsTab = true,
                TextWrapping = TextWrapping.Wrap
            });
        }

        private void SavePdf()
        {
            foreach (FrameworkElement chart in Controls)
                AddControlToPdf(chart);


            var renderer = new HtmlToPdf();
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var renderPath = Path.Combine(path, string.Format("Rapport {0}.pdf", SelectedFestival.FestivalName));

            try
            {
                renderer.RenderHtmlAsPdf(_pdfHtml).SaveAs(renderPath);
                OpenPopup($"Het rapport is succesvol gegeneerd! Het is opgeslagen op je desktop onder de naam: Rapport {SelectedFestival.FestivalName}.pdf");
            }
            catch (IOException)
            {
                OpenPopup("Er is een fout opgetreden tijdens het schrijven van het rapport. Controleer of het bestand gesloten is en je toegang hebt om bestanden te generen");
            }
        }


        private void AddAnswers(ICollection<Answer> answers)
        {
            foreach (Answer answer in answers)
                if (answer is FileAnswer fileAnswer)
                {
                    DateTime date = fileAnswer.CreatedAt;
                    Label label = CreateLabel(date.ToString());
                    Image image = CreateImage(fileAnswer);

                    if (image != null)
                    {
                        Controls.Add(label);
                        Controls.Add(image);
                    }
                }
                else if (answer is StringAnswer stringAnswer)
                    Controls.Add(CreateTextboxFromStringAnswer(stringAnswer));
        }


        private Image CreateImage(FileAnswer answer)
        {
            var image = new Image();

            if (answer.UploadedFilePath == null)
                return null;

            var baseUri = new Uri("http://localhost:5000");
            var source = new BitmapImage(new Uri(baseUri, answer.UploadedFilePath));


            image.Source = source;
            imageSources.Add(image, source.UriSource.ToString());
            return image;
        }

        private void AddControlToPdf(FrameworkElement control)
        {
            if (control is TextBox textBox)
                _pdfHtml += string.Format("<p>{0}</p>", textBox.Text.Replace("\n", "<br>"));
            else if (control is Label label)
                _pdfHtml += string.Format("<h2>{0}</h2>", label.Content);
            else if (control is Image image)
                _pdfHtml += string.Format("<img src='{0}' style='max-width: 100%; height: auto;'>", imageSources[image]);
            else
                _pdfHtml += string.Format("<img src='{0}'>", WriteToPng(control));
        }

        public static string WriteToPng(UIElement element)
        {
            var rect = new Rect(element.RenderSize);
            var visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
                dc.DrawRectangle(new VisualBrush(element), null, rect);


            var bitmap = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
            bitmap.Render(visual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            string fileName = Path.GetTempPath() + Guid.NewGuid() + ".png";

            using (FileStream file = File.OpenWrite(fileName))
                encoder.Save(file);

            return fileName;
        }

        private Label CreateLabel(string text)
        {

            return new Label
            {
                Content = text,
                Width = double.NaN,
                Height = 30
            };
        }


        private TextBox CreateTextboxFromStringAnswer(StringAnswer answer)
        {
            return new TextBox
            {
                Height = 150,
                Width = 700,
                Margin = new Thickness(10),
                AcceptsReturn = true,
                AcceptsTab = true,
                TextWrapping = TextWrapping.Wrap,
                Text = answer.AnswerContents.Replace("\n", "<br>")
            };
        }
    }
}