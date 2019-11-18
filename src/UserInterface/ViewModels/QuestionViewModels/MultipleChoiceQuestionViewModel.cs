using Festispec.Models.Questions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Festispec.UI.ViewModels.QuestionViewModels
{
    class MultipleChoiceQuestionViewModel : ViewModelBase
    {

      
        public ICommand AddOptionCommand { get; set; }


        public MultipleChoiceQuestionViewModel()
        {
            AddOptionCommand = new RelayCommand(AddOption);
        }

        public void AddOption()
        {

            return;
        }


    }
}
