﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Microsoft.Phone.Controls;
using PhoneToolkitSample.Data;

namespace PhoneToolkitSample.Samples
{
    public partial class TransitionsSample : BasePage
    {
        public TransitionsSample()
        {
            DataContext = new TransitionsSampleViewModel();
            InitializeComponent();
        }

        private RotateTransition RotateTransitionElement(string mode)
        {
            RotateTransitionMode rotateTransitionMode = (RotateTransitionMode)Enum.Parse(typeof(RotateTransitionMode), mode, false);
            return new RotateTransition { Mode = rotateTransitionMode };
        }

        private SlideTransition SlideTransitionElement(string mode)
        {
            SlideTransitionMode slideTransitionMode = (SlideTransitionMode)Enum.Parse(typeof(SlideTransitionMode), mode, false);
            return new SlideTransition { Mode = slideTransitionMode };
        }

        private SwivelTransition SwivelTransitionElement(string mode)
        {
            SwivelTransitionMode swivelTransitionMode = (SwivelTransitionMode)Enum.Parse(typeof(SwivelTransitionMode), mode, false);
            return new SwivelTransition { Mode = swivelTransitionMode };
        }

        private TurnstileTransition TurnstileTransitionElement(string mode)
        {
            TurnstileTransitionMode turnstileTransitionMode = (TurnstileTransitionMode)Enum.Parse(typeof(TurnstileTransitionMode), mode, false);
            return new TurnstileTransition { Mode = turnstileTransitionMode };
        }

        private TransitionElement TransitionElement(string family, string mode)
        {
            switch (family)
            {
                case "Rotate":
                    return RotateTransitionElement(mode);
                case "Slide":
                    return SlideTransitionElement(mode);
                case "Swivel":
                    return SwivelTransitionElement(mode);
                case "Turnstile":
                    return TurnstileTransitionElement(mode);
            }
            return null;
        }

        private void See(object sender, RoutedEventArgs e)
        {
            string family = (string)Family.SelectedItem;
            string mode = (string)Mode.SelectedItem;
            TransitionElement transitionElement = null;
            if (family.Equals("Roll"))
            {
                transitionElement = new RollTransition();
            }
            else
            {
                transitionElement = TransitionElement(family, mode);
            }
            PhoneApplicationPage phoneApplicationPage = (PhoneApplicationPage)(((PhoneApplicationFrame)Application.Current.RootVisual)).Content;
            ITransition transition = transitionElement.GetTransition(phoneApplicationPage);
            transition.Completed += delegate
            {
                transition.Stop();
            };
            transition.Begin();
        }

        private void Forward(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Samples/NavigationTransitionSample1.xaml", UriKind.Relative));
        }

        private void FamilySelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string family = (string)Family.SelectedItem;
            Mode.Visibility = family.Equals("Roll") ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    public class TransitionsSampleViewModel : BindableBase
    {
        private IList<string> _families = new List<string>
        {
            "Roll",
            "Rotate",
            "Slide",
            "Swivel",
            "Turnstile"
        };
        public IList<string> Families
        {
            get { return _families; }
        }

        public string Family
        {
            get { return Families[FamilyIndex]; }
        }

        private int _familyIndex = 4;
        public int FamilyIndex
        {
            get { return _familyIndex; }
            set
            {
                if (SetProperty(ref _familyIndex, value))
                {
                    OnPropertyChanged("Family");
                    ModeIndex = Family.Equals("Roll") ? -1 : 0;
                }
            }
        }

        private int _modeIndex;
        public int ModeIndex
        {
            get { return _modeIndex; }
            set { SetProperty(ref _modeIndex, value); }
        }
    }

    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            if (s == null)
            {
                return null;
            }
            switch (s)
            {
                case "Roll":
                    return new List<string>();
                case "Rotate":
                    return Enum.GetNames(typeof(RotateTransitionMode));
                case "Slide":
                    return Enum.GetNames(typeof(SlideTransitionMode));
                case "Swivel":
                    return Enum.GetNames(typeof(SwivelTransitionMode));
                case "Turnstile":
                    return Enum.GetNames(typeof(TurnstileTransitionMode));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}