using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DentistApp.DAL;
using DentistApp.BL;
using System.Windows;
using System.Windows.Input;
using DentistApp.Model;
using Microsoft.Practices.Prism.Commands;

namespace DentistApp.UI.ViewModels
{
    public class OperationViewModel : BaseViewModel
    {
        Operation _Operation;
        #region Properties
        /////properties////
        OperationController OperationController{get;set;}
        //list of Operations
        public ObservableCollection<Operation> Operations { get; set; }
        public Operation Operation { 
            get { 
                return _Operation; 
            } 
            set { 
                _Operation = value; 
                RaisePropertyChanged("Operation"); 
            } 
        }
        #endregion

        #region Commands
        /////commands/////
        public DelegateCommand<object> LoadOperationsCommand { get; private set; }
        public DelegateCommand<object> UpdateOperationCommand { get; private set; }
        public DelegateCommand<object> DeleteOperationCommand { get; private set; }



        #endregion






        public OperationViewModel()
        {
                OperationController = new OperationController();

                LoadOperationsCommand = new DelegateCommand<object>(LoadOperations, (object o)=>{return true;});
                UpdateOperationCommand = new DelegateCommand<object>(UpdateOperation, (object o) => { return true; });
                DeleteOperationCommand = new DelegateCommand<object>(DeleteOperation, (object o) => { return true; });

                LoadOperationsCommand.Execute(null);
        }

        public void LoadOperations(object o)
        {
            var apps = OperationController.List();
            Operations = new ObservableCollection<Operation>(apps);
            RaisePropertyChanged("Operations");
        }
        public void UpdateOperation(object o)
        {
            var operation = o as Operation;

            OperationController.SaveOperation(operation);
            LoadOperations(null);
        }
        public void DeleteOperation(object o)
        {
            var operation = o as Operation;
            OperationController.Delete(Operation);
            LoadOperations(null);
        }
        

    }
}
