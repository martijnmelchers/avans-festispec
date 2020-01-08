using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Festispec.UI.Views.Controls;
using GalaSoft.MvvmLight;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using IronPdf;
using System.Windows.Input;
using Festispec.Models.Answers;
using Festispec.UI.Interfaces;
using System.Linq;
using System.Data.Entity;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels
{
    public class RapportPreviewViewModel: ViewModelBase
    {
        private IQuestionService _questionService;
        private IQuestionnaireService _questionnaireService;
        private IFrameNavigationService _navigationService;
        private IFestivalService _festivalService;
        public ObservableCollection<FrameworkElement> Charts { get; set; }
        public Festival selectedFestival { get; set; }
        public string DescriptionText { get; set; }
        private string PdfHtml = "";



        public ICommand GeneratePdfCommand { get; set; }
        public ICommand BackCommand { get; set; }


        private Dictionary<Image, string> imageSources = new Dictionary<Image, string>();


        public RapportPreviewViewModel(IFrameNavigationService navigationService, IQuestionService questionService, IQuestionnaireService questionnaireService, IFestivalService festivalService)
        {
            _festivalService = festivalService;
            _questionService = questionService;
            _questionnaireService = questionnaireService;
            _navigationService = navigationService;
            GeneratePdfCommand = new RelayCommand(SavePdf);

            BackCommand = new RelayCommand(Back);

            selectedFestival = _festivalService.GetFestival((int)_navigationService.Parameter);
            PdfHtml = "";
            GenerateReport();
        }


        private void ResetReport()
        {
            PdfHtml = "";
            ReportHeading();
        }

        private void ReportHeading()
        {
            PdfHtml += String.Format("<h1>Rapport {0}</h1>", selectedFestival.FestivalName);
        }

        private async void GenerateReport()
        {
            var questionaireId = selectedFestival.Questionnaires.FirstOrDefault().Id;
            var questions = _questionnaireService.GetQuestionsFromQuestionnaire(questionaireId);
            Charts = new ObservableCollection<FrameworkElement>();

            var textBox = new TextBox { Height = 150, Width = 700, Margin = new Thickness(10), AcceptsReturn = true, AcceptsTab = true, TextWrapping = TextWrapping.Wrap };

            ReportHeading();

            Charts.Add(CreateLabel("Beschrijving"));
            Charts.Add(textBox);

            foreach (var question in questions)
                AddQuestionToReport(question);

        }

        private void Back()
        {
            ResetReport();
            _navigationService.NavigateTo("FestivalInfo", selectedFestival.Id);
        }

        private void AddQuestionToReport(Question question)
        {

            // Make sure that the question point to its reference when available.
            if(question is ReferenceQuestion)
                question = ((ReferenceQuestion)question).Question;

            var converter = new GraphSelectorFactory().GetConverter(question.GraphType);
            if (converter == null && !(question is UploadPictureQuestion || question is StringQuestion))
                return;

            else if (question is UploadPictureQuestion || question is StringQuestion)
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
           

                else if (question is StringQuestion)
                {
                    // String.
                    Charts.Add(CreateLabel(question.Contents));
                    AddAnswers(question.Answers);

                    return;
                }
            }

            var chartValues = converter.TypeToChart(question);

            if (chartValues.Count < 1)
                return;

            var lineControl = new Control();
        

            if (question.GraphType == Models.GraphType.Line)
                lineControl = new LineChartControl(chartValues);
            if (question.GraphType == Models.GraphType.Pie)
                lineControl = new PieChartControl(chartValues);
            else if (question.GraphType == Models.GraphType.Column)
                lineControl = new ColumnChartControl(chartValues);


            var textBox = new TextBox { Height = 150, Width = 700, Margin = new Thickness(10), AcceptsReturn = true, AcceptsTab = true, TextWrapping = TextWrapping.Wrap };
            Charts.Add(CreateLabel(question.Contents));


            lineControl.Height = 300;
            lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            Charts.Add(lineControl);
            
            Charts.Add(textBox);
        }

        private void SavePdf()
        {
            foreach(var chart in Charts)            
               AddControlToPdf(chart);
            

            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var renderPath = Path.Combine(path, String.Format("Rapport {0}.pdf", selectedFestival.FestivalName));

            try
            {
                Renderer.RenderHtmlAsPdf(PdfHtml).SaveAs(renderPath);
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
            foreach(var answer in answers)
            {
                if(answer is FileAnswer)
                {
                    var fileAnswer = (FileAnswer)answer;
                    var date = fileAnswer.CreatedAt;
                    var label = CreateLabel(date.ToString());
                    var image = CreateImage(fileAnswer);


                    if(image != null)
                    {
                        Charts.Add(label);
                        Charts.Add(image);
                        continue;
                    }
                }

                else if(answer is StringAnswer)
                {
                    var stringAnswer = (StringAnswer)answer;
                    Charts.Add(CreateTextboxFromStringAnswer(stringAnswer));
                }
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
            if(control is TextBox)
            {
                var textBox = (TextBox)control;
                String richText = textBox.Text;
                richText = richText.Replace("\n", "<br>");
                PdfHtml += String.Format("<p>{0}</p>", richText);
            }
            else if(control is Label)
            {
                var label = (Label)control;
                PdfHtml += String.Format("<h2>{0}</h2>", label.Content);
            }
            else if(control is Image)
            {
                var image = (Image)control;
                PdfHtml += String.Format("<img src='{0}' style='max-width: 100%; height: auto;'>", imageSources[image]);
            }
            else
            {
                var file = WriteToPng(control);
                PdfHtml += String.Format("<img src='{0}'>", file);
            }
        }

        public string WriteToPng(UIElement element)
        {
            var rect = new Rect(element.RenderSize);
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
                dc.DrawRectangle(new VisualBrush(element), null, rect);
            

            var bitmap = new RenderTargetBitmap(
                (int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
            bitmap.Render(visual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";

            using (var file = File.OpenWrite(fileName))
                encoder.Save(file);

            return fileName;
        }

        private Label CreateLabel(string text)
        {
            Label label = new Label();

            label.Content = text;

            label.Width = Double.NaN;

            label.Height = 30;

            return label;
        }



        private TextBox CreateTextboxFromStringAnswer(StringAnswer answer)
        {
            var stringAnswer = new TextBox { Height = 150, Width = 700, Margin = new Thickness(10), AcceptsReturn = true, AcceptsTab = true, TextWrapping = TextWrapping.Wrap };

    
            var contents = answer.AnswerContents;
            contents = contents.Replace("\n", "<br>");

            stringAnswer.Text = contents ;

            return stringAnswer;
        }
    }
}
