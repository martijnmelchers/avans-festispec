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
        private readonly IEmployeeService _employeeService;
        private string _pdfHtml;
        
        private readonly Dictionary<Image, string> _imageSources = new Dictionary<Image, string>();
        
        public RapportPreviewViewModel(
            IFrameNavigationService navigationService,
            IQuestionnaireService questionnaireService,
            IFestivalService festivalService,
            IConfiguration config,
            GraphSelectorFactory graphSelector,
            IEmployeeService employeeService
            )
        {
            _questionnaireService = questionnaireService;
            _navigationService = navigationService;
            _graphFactory = graphSelector;
            _config = config;
            _employeeService = employeeService;
            SelectedFestival = festivalService.GetFestival((int)navigationService.Parameter);

            GeneratePdfCommand = new RelayCommand(SavePdf);
            BackCommand = new RelayCommand(Back);

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

            LoadStyles();

            _pdfHtml += $"<img src='{_config["Urls:WebApp"]}/Images/festispec logo.png'>";
            _pdfHtml += $"<h1>Festispec rapportage {SelectedFestival.FestivalName}</h1>";

            CustomerDetails();

            _pdfHtml += "<p><b>Datum</b></p>";
            _pdfHtml += $"<p>{DateTime.Today.ToShortDateString()}</p>";
        }

        private void LoadStyles() {
            _pdfHtml += "<link href='https://fonts.googleapis.com/css?family=Montserrat&display=swap' rel='stylesheet'>";

            _pdfHtml += "<style> * {font-family: 'Montserrat', sans-serif;} </style>";
        }

        private void CustomerDetails()
        {
            _pdfHtml += "<p><b>Klantgegevens</b></p>";
            _pdfHtml += $"<p>Naam: {SelectedFestival.Customer.CustomerName}</p>";
            _pdfHtml += $"<p>Adres: {SelectedFestival.Address}</p>";
            _pdfHtml += $"<p>KvK: {SelectedFestival.Customer.KvkNr}</p>";
        }

        private void GenerateReport()
        {
            Questionnaire questionnaire = SelectedFestival.Questionnaires.FirstOrDefault();

            List<Question> questions = _questionnaireService.GetQuestionsFromQuestionnaire(questionnaire.Id);

            Controls = new ObservableCollection<FrameworkElement>();

            CreateReport();

            Controls.Add(CreateLabel("Beschrijving / Introductie"));
            Controls.Add(new TextBox
            {
                Height = 150,
                Width = 700,
                Margin = new Thickness(10),
                AcceptsReturn = true,
                AcceptsTab = true,
                TextWrapping = TextWrapping.Wrap
            });

            Controls.Add(CreateLabel("Advies"));
            Controls.Add(new TextBox
            {
                Height = 150,
                Width = 700,
                Margin = new Thickness(10),
                AcceptsReturn = true,
                AcceptsTab = true,
                TextWrapping = TextWrapping.Wrap
            });

            Controls.Add(CreateLabel("Vragen"));
            
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

            Control lineControl = question.GraphType switch
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

            int questionnaireId = SelectedFestival.Questionnaires.FirstOrDefault().Id;
            List<Question> questions = _questionnaireService.GetQuestionsFromQuestionnaire(questionnaireId);
            GenerateReadout(questions);

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
                        Label label = CreateLabel($"Inspecteur: {GetEmployee(answer).Name} / {date.ToString()}");
                        Image image = CreateImage(fileAnswer);

                        var textBox = new TextBox
                        {
                            Height = 150,
                            Width = 700,
                            Margin = new Thickness(10),
                            AcceptsReturn = true,
                            AcceptsTab = true,
                            TextWrapping = TextWrapping.Wrap,
                            Text = fileAnswer.AnswerContents,
                            IsEnabled = false
                        };

                        if (image != null)
                        {
                            Controls.Add(label);
                            Controls.Add(image);
                            Controls.Add(textBox);
                        }   

                        break;
                    }
                    case StringAnswer stringAnswer:
                        Controls.Add(CreateTextBoxFromStringAnswer(stringAnswer));
                        break;
                }
        }

        private Image CreateImage(FileAnswer answer)
        {
            var image = new Image();

            if (answer.UploadedFilePath == null)
                return null;


            try
            {
                var baseUri = new Uri(_config["Urls:WebApp"]);
                var source = new BitmapImage(new Uri(baseUri, answer.UploadedFilePath));

                image.Source = source;
                _imageSources.Add(image, source.UriSource.ToString());
                return image;
            }

            catch (Exception)
            { 
                OpenValidationPopup("Afbeelding is niet gevonden, vraag aan de administrator voor hulp.");
                return null;
            }
        }

        private Employee GetEmployee(Answer answer)
        {
            return _employeeService.GetEmployee(answer.PlannedInspection.Employee.Id);
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
        
        private static TextBox CreateTextBoxFromStringAnswer(StringAnswer answer)
        {
            return new TextBox
            {
                Height = 150,
                Width = 700,
                Margin = new Thickness(10),
                AcceptsReturn = true,
                AcceptsTab = true,
                TextWrapping = TextWrapping.Wrap,
                Text = answer.AnswerContents.Replace("\n", "<br>"),
                IsEnabled = false
            };
        }

        private void GenerateReadout(IEnumerable<Question> questions)
        {
            _pdfHtml += "<div style='page-break-after: always;'></div>";
            _pdfHtml += "<h1>Bijlage</h1>";
            _pdfHtml += "<h2>Ruwe data</h2>";
            foreach(Question question in questions)
            {
                _pdfHtml += $"<p style='font-weight: bold;'>{question.Contents}</p>";
                ReadoutAnswers(question.Answers.ToList());
            }
        }

        private void ReadoutAnswers(IEnumerable<Answer> answers)
        {
            foreach(Answer answer in answers)
            {
                _pdfHtml += answer switch
                {
                    FileAnswer fileAnswer =>  $"<p>{GetEmployee(fileAnswer).Name}: {fileAnswer.UploadedFilePath}",
                    StringAnswer stringAnswer => $"<p>{GetEmployee(stringAnswer).Name}: {stringAnswer.AnswerContents}</p>",
                    NumericAnswer numericAnswer => $"<p>{GetEmployee(numericAnswer).Name}: {numericAnswer.IntAnswer}</p>",
                    MultipleChoiceAnswer multiplechoiceAnswer => $"<p>{GetEmployee(multiplechoiceAnswer).Name}: {((MultipleChoiceQuestion)multiplechoiceAnswer.Question).OptionCollection[multiplechoiceAnswer.MultipleChoiceAnswerKey].Value}</p>",
                    _ => ""
                };
            }
        }

        private void Back()
        {
            _navigationService.NavigateTo("FestivalInfo", SelectedFestival.Id);
        }
    }
}
