using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM.ModelView_View_Model.OpenQuiz
{
    class OpenQuizViewModel
    {
        public string Question
        {
            get; set;
        }
        public string FirstAnswer
        {
            get; set;
        }
        public string SecondAnswer
        {
            get; set;
        }
        public string ThirdAnswer
        {
            get; set;
        }
        public string FourthAnswer
        {
            get; set;
        }
        event Action SubmitAnswer;
        event Action CheckAnswer;
    }
}
