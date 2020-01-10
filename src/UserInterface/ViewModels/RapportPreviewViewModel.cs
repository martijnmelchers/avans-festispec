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
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using Festispec.UI.Interfaces;
using Festispec.UI.Views.Controls;
using GalaSoft.MvvmLight.Command;
using IronPdf;
using Microsoft.Extensions.Configuration;

namespace Festispec.UI.ViewModels
{
    public class RapportPreviewViewModel : BaseValidationViewModel
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly GraphSelectorFactory _graphFactory;
        private readonly IFrameNavigationService _navigationService;
        private readonly IConfiguration _config;
        private string _pdfHtml;
        
        private readonly Dictionary<Image, string> _imageSources = new Dictionary<Image, string>();
        
        public RapportPreviewViewModel(
            IFrameNavigationService navigationService,
            IQuestionnaireService questionnaireService,
            IFestivalService festivalService,
            IConfiguration config,
            GraphSelectorFactory graphSelector
            )
        {
            _questionnaireService = questionnaireService;
            _navigationService = navigationService;
            _graphFactory = graphSelector;
            _config = config;

            GeneratePdfCommand = new RelayCommand(SavePdf);
            BackCommand = new RelayCommand(() => navigationService.NavigateTo("FestivalInfo", SelectedFestival.Id));

            SelectedFestival = festivalService.GetFestival((int)navigationService.Parameter);

            GenerateReport();

        }

        public ObservableCollection<FrameworkElement> Controls { get; set; }
        public Festival SelectedFestival { get; set; }


        public ICommand GeneratePdfCommand { get; set; }
        public ICommand BackCommand { get; set; }


        private void CreateReport()
        {
            // this has been done deliberately.
            _pdfHtml = "";
            _pdfHtml += $"<h1>Rapport {SelectedFestival.FestivalName}</h1>";
        }

        private void GenerateReport()
        {
            if(SelectedFestival.Questionnaires.Count == 0)
            {
                _navigationService.NavigateTo("FestivalInfo", SelectedFestival.Id);
                return;
            }


            int questionnaireId = SelectedFestival.Questionnaires.FirstOrDefault().Id;
            List<Question> questions = _questionnaireService.GetQuestionsFromQuestionnaire(questionnaireId);

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

        private void AddQuestionToReport(Question question)
        {
            // Make sure that the question point to its reference when available.
            if (question is ReferenceQuestion referenceQuestion)
                question = referenceQuestion.Question;

            IGraphable converter = _graphFactory.GetConverter(question.GraphType);

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

            var lineControl = question.GraphType switch
            {
                GraphType.Line => new LineChartControl(chartValues),
                GraphType.Pie => new PieChartControl(chartValues),
                GraphType.Column => new ColumnChartControl(chartValues),
                _ => new Control()
            };

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
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string renderPath = Path.Combine(path, $"Rapport {SelectedFestival.FestivalName}.pdf");

            try
            {
                renderer.RenderHtmlAsPdf(_pdfHtml).SaveAs(renderPath);
                OpenValidationPopup($"Het rapport is succesvol gegeneerd! Het is opgeslagen op je desktop onder de naam: Rapport {SelectedFestival.FestivalName}.pdf");
            }
            catch (IOException)
            {
                OpenValidationPopup("Er is een fout opgetreden tijdens het schrijven van het rapport. Controleer of het bestand gesloten is en je toegang hebt om bestanden te generen");
            }
        }


        private void AddAnswers(IEnumerable<Answer> answers)
        {
            foreach (Answer answer in answers)
                switch (answer)
                {
                    case FileAnswer fileAnswer:
                    {
                        DateTime date = fileAnswer.CreatedAt;
                        Label label = CreateLabel(date.ToString());
                        Image image = CreateImage(fileAnswer);

                        if (image != null)
                        {
                            Controls.Add(label);
                            Controls.Add(image);
                        }

                        break;
                    }
                    case StringAnswer stringAnswer:
                        Controls.Add(CreateTextboxFromStringAnswer(stringAnswer));
                        break;
                }
        }

        private Image CreateImage(FileAnswer answer)
        {
            var image = new Image();

            if (answer.UploadedFilePath == null)
                return null;

            var baseUri = new Uri(_config["Urls:WebApp"]);
            var source = new BitmapImage(new Uri(baseUri, answer.UploadedFilePath));
            
            image.Source = source;
            _imageSources.Add(image, source.UriSource.ToString());
            return image;
        }

        private void AddControlToPdf(FrameworkElement control)
        {
            _pdfHtml += control switch
            {
                TextBox textBox => $"<p>{textBox.Text.Replace("\n", "<br>")}</p>",
                Label label => $"<h2>{label.Content}</h2>",
                Image image => $"<img src='{_imageSources[image]}' style='max-width: 100%; height: auto;'>",
                _ => $"<img src='{WriteToPng(control)}'>"
            };
        }

        private static string WriteToPng(UIElement element)
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

        private static Label CreateLabel(string text)
        {
            return new Label
            {
                Content = text,
                Width = double.NaN,
                Height = 30
            };
        }
        
        private static TextBox CreateTextboxFromStringAnswer(StringAnswer answer)
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