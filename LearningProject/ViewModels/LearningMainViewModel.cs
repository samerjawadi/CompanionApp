using LearningProject.Models;
using LearningProject.Models.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Resources;

namespace LearningProject.ViewModels
{
    public class LearningMainViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        /// <summary>/// Prism Property/// </summary>
		private ObservableCollection<Exemples> _exempelsList;

        public ObservableCollection<Exemples> ExempelsList
        {
            get { return _exempelsList; }
            set { SetProperty(ref _exempelsList, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public LearningMainViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PDFSelectedEvent>().Subscribe(PDFSelectedMethod);

            initList();
            PDFSelectedMethod("Commande Boutons");

        }

        private void PDFSelectedMethod(string obj)
        {
            foreach(var e in _exempelsList)
            {
                if(e.Name == obj)
                {
                    e.IsSelected = true;
                    _eventAggregator.GetEvent<LoadPDFEvent>().Publish(e.Path);
                    Title = e.Name;
                }
                else
                {
                    e.IsSelected = false;
                }
            }
        }

        

        private void initList()
        {
            ExempelsList = new ObservableCollection<Exemples>();
            ExempelsList.Add(new Exemples("Commande Boutons", "Commande_Boutons.pdf", _eventAggregator));
            ExempelsList.Add(new Exemples("Suivie de ligne", "Suivie_de_ligne.pdf", _eventAggregator));
            ExempelsList.Add(new Exemples("Alerte de chute", "Alerte_de_chute.pdf", _eventAggregator));
            ExempelsList.Add(new Exemples("Controle de temperature", "Controle_de_temperature.pdf", _eventAggregator));
            ExempelsList.Add(new Exemples("Evitement d'obstacles", "Evitement_d'obstacles.pdf", _eventAggregator));
            ExempelsList.Add(new Exemples("Suivi intelligent", "Suivi_intelligent.pdf", _eventAggregator));

        }
    }
}
