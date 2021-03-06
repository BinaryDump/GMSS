﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Input;

namespace GiveMeSomeSheets
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
      
        public MainWindowViewModel()
        {
            CheckIfAlreadyLaunched();
            NbSheets = 1;
            PrinterList = new ObservableCollection<string>();
            GetPrinterList();
            SendToPrinterButton = new RelayCommand(new Action<object>(SendToPrinterCommand));
        }

        private int _nbSheets;
        private ICommand _sendToPrinterButton;
        private ObservableCollection<string> _printerList;
        private string _cBPrinter;

        #region ## getters and setters ##
        public int NbSheets
        {
            get;
            set;
        }

        public ObservableCollection<string> PrinterList
        {
            get;
            set;
        }

        public string CBPrinter
        {
            get
            {
                return _cBPrinter;
            }
            set
            {
                _cBPrinter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CBPrinter"));
            }
        }

        public ICommand SendToPrinterButton
        {
            get
            {
                return _sendToPrinterButton;
            }
            set
            {
                _sendToPrinterButton = value;
            }
        }
        #endregion

        #region ## methods ##
        public void SendToPrinterCommand(object o)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = CBPrinter;

            if (pd.PrinterSettings.IsValid)
            {
                for (int i = 0; i < NbSheets; i++)
                {
                    pd.Print();
                }
            }else
            {
                MessageBox.Show("An error occured.\nPlease check that your printer is active.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            //foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters) //Get installed printers 
            //{
            //    MessageBox.Show(printer);
            //}
        }


        public void GetPrinterList()
        {
            for (int count = 0; count < PrinterSettings.InstalledPrinters.Count; count++)
            {
                PrinterList.Add(PrinterSettings.InstalledPrinters[count]);
            }
        }

        public void CheckIfAlreadyLaunched()
        {
            if(Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show("Application is already running.", "Already running.", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
                return;
            }
        }

        #endregion

        #region ## PropertyChanged ##
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

    }
}
