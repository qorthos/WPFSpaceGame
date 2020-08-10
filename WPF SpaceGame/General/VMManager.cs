using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.General
{
    public class VMManager : ObservableObject
    {
        List<ViewModel> viewModels;

        private ViewModel _selectedViewModel;
        private ViewModel lastViewModel;
        private bool _isBusy;


        #region PROPERTIES


        public ViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value; RaisePropertyChanged(nameof(SelectedViewModel)); }
        }


        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value; RaisePropertyChanged(nameof(IsBusy));
            }
        }


        #endregion

        public VMManager()
        {
            viewModels = new List<ViewModel>();
        }


        public void AddVM(ViewModel vm)
        {
            viewModels.Add(vm);
        }



        public T GetVM<T>() where T : ViewModel, new()
        {
            Type type = typeof(T);

            foreach (ViewModel vm in viewModels)
            {
                if (vm is T)
                {
                    return (T)vm;
                }
            }

            var newVM = new T();
            viewModels.Add(newVM);

            return newVM;
        }


        public void TransitionToVM(ViewModel nextVM)
        {
            lastViewModel = _selectedViewModel;
            SelectedViewModel = nextVM;
            SelectedViewModel.Focused();
        }


        public void TransitionToVM<T>() where T : ViewModel, new()
        {
            lastViewModel = _selectedViewModel;
            SelectedViewModel = GetVM<T>();
            SelectedViewModel.Focused();
        }


        public void GoBack()
        {
            TransitionToVM(lastViewModel);
        }
    }
}
