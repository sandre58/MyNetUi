// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Busy.Models
{
    public class DeterminateBusy : Busy
    {
        public string? Message { get; set; }

        public double Value { get; set; }

        public double Maximum { get; set; } = 1;

        public double Minimum { get; set; }
    }
}
