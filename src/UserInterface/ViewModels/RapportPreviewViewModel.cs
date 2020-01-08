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
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using Festispec.UI.Interfaces;
using Festispec.UI.Views.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronPdf;

namespace Festispec.UI.ViewModels
{
    public class RapportPreviewViewModel : ViewModelBase
    {
        private readonly IFestivalService _festivalService;
        private readonly IFrameNavigationService _navigationService;
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IQuestionService _questionService;
        private string _pdfHtml;


        private readonly Dictionary<Image, string> imageSources = new Dictionary<Image, string>();


        public RapportPreviewViewModel(IFrameNavigationService navigationService, IQuestionService questionService,
            IQuestionnaireService questionnaireService, IFestivalService festivalService)
        {
            _festivalService = festivalService;
            _questionService = questionService;
            _questionnaireService = questionnaireService;
            _navigationService = navigationService;
            GeneratePdfCommand = new RelayCommand(SavePdf);

            BackCommand = new RelayCommand(Back);

            SelectedFestival = _festivalService.GetFestival((int) _navigationService.Parameter);
            _pdfHtml = "";
            GenerateReport();
        }

        public ObservableCollection<FrameworkElement> Charts { get; set; }
        public Festival SelectedFestival { get; set; }
        public string DescriptionText { get; set; }


        public ICommand GeneratePdfCommand { get; set; }
        public ICommand BackCommand { get; set; }


        private void ResetReport()
        {
            _pdfHtml = "";
            ReportHeading();
        }

        private void ReportHeading()
        {
            _pdfHtml += string.Format("<h1>Rapport {0}</h1>", SelectedFestival.FestivalName);
        }

        private async void GenerateReport()
        {
            int questionaireId = SelectedFestival.Questionnaires.FirstOrDefault().Id;
            List<Question> questions = _questionnaireService.GetQuestionsFromQuestionnaire(questionaireId);
            Charts = new ObservableCollection<FrameworkElement>();

            var textBox = new TextBox
            {
                Height = 150, Width = 700, Margin = new Thickness(10), AcceptsReturn = true, AcceptsTab = true,
                TextWrapping = TextWrapping.Wrap
            };

            ReportHeading();

            Charts.Add(CreateLabel("Beschrijving"));
            Charts.Add(textBox);

            foreach (Question question in questions)
                AddQuestionToReport(question);
        }

        private void Back()
        {
            ResetReport();
            _navigationService.NavigateTo("FestivalInfo", SelectedFestival.Id);
        }

        private void AddQuestionToReport(Question question)
        {
            // Make sure that the question point to its reference when available.
            if (question is ReferenceQuestion)
                question = ((ReferenceQuestion) question).Question;

            IGraphable converter = new GraphSelectorFactory().GetConverter(question.GraphType);
            if (converter == null && !(question is UploadPictureQuestion || question is StringQuestion)) return;

            if (question is UploadPictureQuestion || question is StringQuestion)
            {
                // String question or image upload.
                if (question is UploadPictureQuestion)
                {
                    Charts.Add(CreateLabel(question.Contents));
                    // UploadPictureQuestion uploadQuestion = (UploadPictureQuestion)question;
                    //var fileAnwers = ((ICollection<FileAnswer>)uploadQuestion.Answers).ToList();

                    AddAnswers(question.Answers);
                    return;
                }


                if (question is StringQuestion)
                {
                    // String.
                    Charts.Add(CreateLabel(question.Contents));
                    AddAnswers(question.Answers);

                    return;
                }
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


            var textBox = new TextBox
            {
                Height = 150, Width = 700, Margin = new Thickness(10), AcceptsReturn = true, AcceptsTab = true,
                TextWrapping = TextWrapping.Wrap
            };
            Charts.Add(CreateLabel(question.Contents));


            lineControl.Height = 300;
            lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            Charts.Add(lineControl);

            Charts.Add(textBox);
        }

        private void SavePdf()
        {
            foreach (FrameworkElement chart in Charts)
                AddControlToPdf(chart);


            var Renderer = new HtmlToPdf();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string renderPath = Path.Combine(path, string.Format("Rapport {0}.pdf", SelectedFestival.FestivalName));

            try
            {
                Renderer.RenderHtmlAsPdf(_pdfHtml).SaveAs(renderPath);
                MessageBox.Show("Generated!");
            }
            catch (IOException)
            {
                MessageBox.Show("Sluit het pdf bestand in de editor. voor het opslaan.");
                ResetReport();
            }
        }


        private void AddAnswers(ICollection<Answer> answers)
        {
            foreach (Answer answer in answers)
                if (answer is FileAnswer)
                {
                    var fileAnswer = (FileAnswer) answer;
                    DateTime date = fileAnswer.CreatedAt;
                    Label label = CreateLabel(date.ToString());
                    Image image = CreateImage(fileAnswer);


                    if (image != null)
                    {
                        Charts.Add(label);
                        Charts.Add(image);
                    }
                }

                else if (answer is StringAnswer)
                {
                    var stringAnswer = (StringAnswer) answer;
                    Charts.Add(CreateTextboxFromStringAnswer(stringAnswer));
                }
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
            if (control is TextBox)
            {
                var textBox = (TextBox) control;
                string richText = textBox.Text;
                richText = richText.Replace("\n", "<br>");
                _pdfHtml += string.Format("<p>{0}</p>", richText);
            }
            else if (control is Label)
            {
                var label = (Label) control;
                _pdfHtml += string.Format("<h2>{0}</h2>", label.Content);
            }
            else if (control is Image)
            {
                var image = (Image) control;
                _pdfHtml += string.Format("<img src='{0}' style='max-width: 100%; height: auto;'>",
                    imageSources[image]);
            }
            else
            {
                string file = WriteToPng(control);
                _pdfHtml += string.Format("<img src='{0}'>", file);
            }
        }

        public string WriteToPng(UIElement element)
        {
            var rect = new Rect(element.RenderSize);
            var visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawRectangle(new VisualBrush(element), null, rect);
            }


            var bitmap = new RenderTargetBitmap(
                (int) rect.Width, (int) rect.Height, 96, 96, PixelFormats.Default);
            bitmap.Render(visual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            string fileName = Path.GetTempPath() + Guid.NewGuid() + ".png";

            using (FileStream file = File.OpenWrite(fileName))
            {
                encoder.Save(file);
            }

            return fileName;
        }

        private Label CreateLabel(string text)
        {
            var label = new Label();

            label.Content = text;

            label.Width = double.NaN;

            label.Height = 30;

            return label;
        }


        private TextBox CreateTextboxFromStringAnswer(StringAnswer answer)
        {
            var stringAnswer = new TextBox
            {
                Height = 150, Width = 700, Margin = new Thickness(10), AcceptsReturn = true, AcceptsTab = true,
                TextWrapping = TextWrapping.Wrap
            };


            string contents = answer.AnswerContents;
            contents = contents.Replace("\n", "<br>");

            stringAnswer.Text = contents;

            return stringAnswer;
        }
    }
}