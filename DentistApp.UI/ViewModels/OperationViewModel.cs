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
using System.ComponentModel;
using System.Windows.Controls;

namespace DentistApp.UI.ViewModels
{
    public class OperationViewModel : BaseViewModel
    {
        Operation _Operation;
        #region Properties
        /////properties////
        OperationController OperationController{get;set;}
        //list of Operations
        public BindingList<Operation> Operations { get; set; }
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
        public DelegateCommand<object> EditRowCommand { get; set; }


        #endregion






        public OperationViewModel()
        {
                OperationController = new OperationController();

                LoadOperationsCommand = new DelegateCommand<object>(LoadOperations, (object o)=>{return true;});
                UpdateOperationCommand = new DelegateCommand<object>(UpdateOperation, (object o) => { return true; });
                DeleteOperationCommand = new DelegateCommand<object>(DeleteOperation, (object o) => { return true; });
            EditRowCommand = new DelegateCommand<object>(MyDataGrid_RowEditEnding, (object o) => { return true; });
            
                LoadOperationsCommand.Execute(null);
        }

        public void LoadOperations(object o)
        {
            IList<Operation> apps = (IList<Operation>)OperationController.List();
            Operations = new BindingList<Operation>(apps);
            RaisePropertyChanged("Operations");
        }
        public void UpdateOperation(object o)
        {
            var operation = o as Operation;
            if (operation == null || string.IsNullOrWhiteSpace(operation.Description)) { MessageBox.Show("Please enter a description, click outside and then press Save again."); return; }
            OperationController.SaveOperation(operation);
            LoadOperations(null);
        }

        private void MyDataGrid_RowEditEnding(object sender) 
        {    // Only act on Commit
   
        }

        public void DeleteOperation(object o)
        {
            if (!ShouldDelete()) return;
            var operation = o as Operation;
            OperationController.Delete(Operation);
            LoadOperations(null);
        }
        

    }
}
