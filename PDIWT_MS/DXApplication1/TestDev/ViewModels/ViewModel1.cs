using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

namespace TestDev.ViewModels
{
    [POCOViewModel]
    public class ViewModel1
    {
        public virtual string Text { get; set; }
        public virtual IMessageBoxService MessageBoxService { get { return null; } }
        public void ShowText()
        {
            MessageBoxService.ShowMessage(Text);
        }
    }
}