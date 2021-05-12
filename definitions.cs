
using System.Collections.Generic;

using hexlist2int = aldlparser.hexlist2int;

using hexlist2str = aldlparser.hexlist2str;

using int2hexlist = aldlparser.int2hexlist;

using System;

using System.Linq;

public static class definitions {
    
    public static Dictionary<string, string> cluster_models = new Dictionary<object, object> {
        {
            "00",
            "Holden"},
        {
            "01",
            "HSV"},
        {
            "02",
            "Chevrolet"},
        {
            "03",
            "Omega"},
        {
            "04",
            "Chevrolet Special Vehicles"},
        {
            "05",
            "Pontiac"},
        {
            "06",
            "Vauxhall"}};
    
    public static Dictionary<string, string> cluster_hsv_type = new Dictionary<object, object> {
        {
            "00",
            "XU6"},
        {
            "01",
            "Clubsport"},
        {
            "02",
            "Clubsport R8"},
        {
            "03",
            "GTS"},
        {
            "04",
            "Senator"},
        {
            "05",
            "Senator Signature"},
        {
            "06",
            "Maloo"},
        {
            "07",
            "Coupe"},
        {
            "08",
            "GTS-R"},
        {
            "09",
            "Build No."},
        {
            "0A",
            "HSV"},
        {
            "0B",
            "Grange"},
        {
            "0C",
            "Maloo R8"},
        {
            "0D",
            "<CUSTOM>"}};
    
    public static Dictionary<int, string> cluster_transmission = new Dictionary<object, object> {
        {
            0x00,
            "Manual"},
        {
            0x01,
            "Automatic"},
        {
            0x02,
            "Manual + Gear indicator"},
        {
            0x03,
            "Manual + PRND321"}};
    
    public static Dictionary<int, string> car_modules = new Dictionary<object, object> {
        {
            0xF1,
            "BCM"},
        {
            0xF9,
            "ABS"},
        {
            0x41,
            "BCM-data"},
        {
            0xF2,
            "Cluster"},
        {
            0x11,
            "Transmission-data"},
        {
            0xA1,
            "Airbag-data"},
        {
            0xFB,
            "Airbag"}};
    
    public static Dictionary<object, object> car_modules_hex = new Dictionary<object, object> {
    };
    
    static definitions() {
        car_modules_hex[v] = k;
    }
    
    public static Dictionary<int, Func<object, object>> cluster_memory_definitions = new Dictionary<object, object> {
        {
            0x33,
            x => ("MPH-brightness", x)},
        {
            0x37,
            x => ("DRL Lamp", x)},
        {
            0x48,
            x => zip(new List<object> {
                "BCMoutput0",
                "BCMoutput64",
                "BCMoutput128",
                "BCMoutput192",
                "BCMoutput255"
            }, x)},
        {
            0x73,
            x => ("IndividualBrightness-dials", x)},
        {
            0x74,
            x => ("IndividualBrightness-pointers", x)},
        {
            0x75,
            x => ("IndividualBrightness-displays", x)},
        {
            0xE4,
            x => ("SideLCDContrast", Convert.ToInt32(x[0], 16))},
        {
            0xE3,
            x => ("CenterLCDContrast", Convert.ToInt32(x[0], 16))},
        {
            0x47,
            x => ("HighTempWarning", Convert.ToInt32(x[0], 16))},
        {
            0x3E,
            x => zip(new List<object> {
                "Temp-L",
                @"Temp-1/4",
                @"Temp-3/8low",
                @"Temp-3/8hi",
                @"Temp-1/2",
                @"Temp-5/8",
                @"Temp-3/4",
                @"Temp-7/8",
                @"Temp-Hi"
            }, (from i in x
                select Convert.ToInt32(i, 16)).ToList())},
        {
            0x7E,
            x => ("Transmission", Enumerable.Range(0, 4 - 0).Contains(hexlist2int(x)) ? transmission[hexlist2int(x)] : x)},
        {
            0x01,
            x => ("Speedo pulse per KM", hexlist2int(x))},
        {
            0x02,
            x => ("Tacho pulse per rev", hexlist2int(x) / 100.0)},
        {
            0x66,
            x => ("SRS-Airbag level", x)},
        {
            0x4D,
            x => ("Mandatory overspeed", hexlist2int(x))},
        {
            0x4F,
            x => ("Brake chime (secs)", hexlist2int(x))},
        {
            0x50,
            x => ("Police Mode", hexlist2int(x) == 1 ? true : false)},
        {
            0x54,
            x => ("Instrument Level", hexlist2int(x))},
        {
            0x60,
            x => ("Cluster serial", hexlist2int(x))},
        {
            0x65,
            x => ("GM Part #", hexlist2int(x))},
        {
            0x86,
            x => ("Overspeed Alarm", hexlist2int(x))},
        {
            0x7F,
            x => ("Startup Logo", models[x[0]])},
        {
            0xCD,
            x => ("Lamp Configuration", bin(hexlist2int(x)))},
        {
            0xDB,
            x => ("Rest reminder (mins)", hexlist2int(x))},
        {
            0x61,
            x => zip(new List<object> {
                "Motor zero offset-speedo",
                "Motor zero offset-tach",
                "Motor zero offset-fuel",
                "Motor zero offset-temp"
            }, new List<object> {
                hexlist2int(x[0::2]),
                hexlist2int(x[2::4]),
                hexlist2int(x[4::6]),
                hexlist2int(x[6::8])
            })},
        {
            0x9A,
            x => zip(new List<object> {
                "Gauge offset-speedo",
                "Gauge offset-tach",
                "Gauge offset-fuel",
                "Gauge offset-temp"
            }, new List<object> {
                hexlist2int(x[0::2]),
                hexlist2int(x[2::4]),
                hexlist2int(x[4::6]),
                hexlist2int(x[6::8])
            })},
        {
            0xAA,
            x => ("Last 6 of VIN", hexlist2int(x))},
        {
            0xA0,
            x => ("HSV Shutdown txt", hsv_type[x[0]])},
        {
            0x9F,
            x => ("HSV Serial Number", hexlist2int(x[0]))},
        {
            0xE6,
            x => ("HSV Custom text", hexlist2str(x))},
        {
            0xCF,
            x => zip(new List<object> {
                "Cold shift light",
                "1st gear shiftlight",
                "2nd gear shiftlight",
                "3rd gear shiftlight",
                "4th gear shiftlight",
                "5th gear shiftlight"
            }, (from i in x
                select (Convert.ToInt32(i, 16) / 2 * 100)).ToList())},
        {
            0x03,
            x => zip(new List<object> {
                "FuelSenderOhm1",
                "FuelSenderOhm2",
                "FuelSenderOhm3",
                "FuelSenderOhm4",
                "FuelSenderOhm5",
                "FuelSenderOhm6",
                "FuelSenderOhm7",
                "FuelSenderOhm8",
                "FuelSenderOhm9",
                "FuelSenderOhm10"
            }, (from i in Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(Convert.ToDouble(x.Count - 1) / 2))).Select(_x_2 => 1 + _x_2 * 2)
                select Convert.ToInt32(x[i], 16)).ToList())},
        {
            0x0D,
            x => zip(new List<object> {
                "FuelSenderVol1",
                "FuelSenderVol2",
                "FuelSenderVol3",
                "FuelSenderVol4",
                "FuelSenderVol5",
                "FuelSenderVol6",
                "FuelSenderVol7",
                "FuelSenderVol8",
                "FuelSenderVol9",
                "FuelSenderVol10"
            }, (from i in x
                select (Convert.ToInt32(i, 16) / 2.0)).ToList())},
        {
            0x80,
            x => zip(new List<object> {
                "Gauge-E",
                "Gauge-1/4",
                "Gauge-1/2",
                "Gauge-3/4",
                "Gauge-F"
            }, (from i in x
                select (Convert.ToInt32(i, 16) / 2.0)).ToList())},
        {
            0x17,
            x => ("Petrol Tank capacity(L)", hexlist2int(x) / 2.0)},
        {
            0x18,
            x => ("Petrol Tank damping const.", hexlist2int(x))},
        {
            0x1C,
            x => ("Low Petrol Warn", hexlist2int(x))},
        {
            0x1D,
            x => ("Very Low Petrol Warn", hexlist2int(x))}};
    
    static definitions() {
        foreach (var _tup_1 in car_modules) {
            k = _tup_1.Item1;
            v = _tup_1.Item2;
        }
    }
}
