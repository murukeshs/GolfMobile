﻿using Golf.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class RoundRules : BaseViewModel
    {
        public string roundRuleId { get; set; }

        public string ruleName { get; set; }

        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                OnPropertyChanged(nameof(Checked));
            }
        }
        private bool _Checked { get; set; } = false;
    }
}
