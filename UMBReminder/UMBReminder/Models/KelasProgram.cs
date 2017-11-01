using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UMBReminder.Models
{
    public class KelasProgram
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDKelasProgram { get; set; }

        public string TahunAjaran { get; set; }

        public string Semester { get; set; }

        public string KelasProgramReg { get; set; }

        public Nullable<DateTime> KRSStart { get; set; }

        public Nullable<DateTime> KRSEnd { get; set; }
        
        public Nullable<DateTime> PembukaanPerkuliahan { get; set; }

        public Nullable<DateTime> CutiEnd { get; set; }

        public Nullable<DateTime> BatalKrsStart { get; set; }

        public Nullable<DateTime> BatalKrsEnd { get; set; }

        public Nullable<DateTime> Perkuliahan1Start { get; set; }

        public Nullable<DateTime> Perkuliahan1End { get; set; }

        public Nullable<DateTime> UTSStart { get; set; }

        public Nullable<DateTime> UTSEnd { get; set; }

        public Nullable<DateTime> Perkuliahan2Start { get; set; }

        public Nullable<DateTime> Perkuliahan2End { get; set; }

        public Nullable<DateTime> UASStart { get; set; }

        public Nullable<DateTime> UASEnd { get; set; }

        public Nullable<DateTime> HasilStudi { get; set; }

        public string Status { get; set; }

        public string KodeKelasProgram { get; set; }
    }
}
