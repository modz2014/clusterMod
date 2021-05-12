cluster_models = {
    "00" : "Holden",
    "01" : "HSV",
    "02" : "Chevrolet",
    "03" : "Omega",
    "04" : "Chevrolet Special Vehicles",
    "05" : "Pontiac",
    "06" : "Vauxhall",
}
cluster_hsv_type = {
    "00" : "XU6",
    "01" : "Clubsport",
    "02" : "Clubsport R8",
    "03" : "GTS",
    "04" : "Senator",
    "05" : "Senator Signature",
    "06" : "Maloo",
    "07" : "Coupe",
    "08" : "GTS-R",
    "09" : "Build No.",
    "0A" : "HSV",
    "0B" : "Grange",
    "0C" : "Maloo R8",
    "0D" : "<CUSTOM>",
}
cluster_transmission = {
    0x00 : "Manual",
    0x01 : "Automatic",
    0x02 : "Manual + Gear indicator",
    0x03 : "Manual + PRND321",
}
car_modules = {
    0xF1 : "BCM",
    0xF9 : "ABS",
    0x41 : "BCM-data",
    0xF2 : "Cluster",
    0x11 : "Transmission-data",
    0xA1 : "Airbag-data",
    0xFB : "Airbag",
}
#invert the car modules dict to allow calling by name
car_modules_hex = {}
for k,v in car_modules.iteritems():
    car_modules_hex[v] = k

from aldlparser import hexlist2int, hexlist2str, int2hexlist
cluster_memory_definitions = {
    #Illumination
    0x33 : lambda x: ("MPH-brightness",x),
    0x37 : lambda x: ("DRL Lamp",x),
    0x48 : lambda x: zip(["BCMoutput0","BCMoutput64",
                        "BCMoutput128","BCMoutput192",
                        "BCMoutput255"],x),
    0x73 : lambda x: ("IndividualBrightness-dials",x),
    0x74 : lambda x: ("IndividualBrightness-pointers",x),
    0x75 : lambda x: ("IndividualBrightness-displays",x),
    0xE4 : lambda x: ("SideLCDContrast",int(x[0],16)),
    0xE3 : lambda x: ("CenterLCDContrast",int(x[0],16)),
    ##Temperatures
    0x47 : lambda x: ("HighTempWarning",int(x[0],16)),
    0x3E : lambda x: zip(["Temp-L",r"Temp-1/4",
                          r"Temp-3/8low",r"Temp-3/8hi",
                          r"Temp-1/2",r"Temp-5/8",r"Temp-3/4",
                          r"Temp-7/8",r"Temp-Hi"],[int(i,16) for i in x] ),
    #Transmission
    0x7E : lambda x: ("Transmission", transmission[hexlist2int(x)] if hexlist2int(x) in range(0,4) else x),
    0x01 : lambda x: ("Speedo pulse per KM", hexlist2int(x)),
    0x02 : lambda x: ("Tacho pulse per rev", hexlist2int(x)/100.),
    #Equipment
    0x66 : lambda x: ("SRS-Airbag level",x),
    #General
    0x4D : lambda x: ("Mandatory overspeed", hexlist2int(x)),
    0x4F : lambda x: ("Brake chime (secs)", hexlist2int(x)),
    0x50 : lambda x: ("Police Mode", True if hexlist2int(x) == 1 else False),
    0x54 : lambda x: ("Instrument Level", hexlist2int(x)),
    0x60 : lambda x: ("Cluster serial", hexlist2int(x)),
    0x65 : lambda x: ("GM Part #", hexlist2int(x)),
    0x86 : lambda x: ("Overspeed Alarm", hexlist2int(x)),
    0x7F : lambda x: ("Startup Logo", models[x[0]]),
    0xCD : lambda x: ("Lamp Configuration", bin(hexlist2int(x))),
    0xDB : lambda x: ("Rest reminder (mins)", hexlist2int(x)),
    0x61 : lambda x: zip(["Motor zero offset-speedo", "Motor zero offset-tach",
                          "Motor zero offset-fuel", "Motor zero offset-temp"],
                        [ hexlist2int(x[0:2]), hexlist2int(x[2:4]), 
                           hexlist2int(x[4:6]), hexlist2int(x[6:8])]
                         ),
    0x9A : lambda x: zip(["Gauge offset-speedo", "Gauge offset-tach",
                          "Gauge offset-fuel", "Gauge offset-temp"],
                        [ hexlist2int(x[0:2]), hexlist2int(x[2:4]), 
                           hexlist2int(x[4:6]), hexlist2int(x[6:8])]
                         ),
    0xAA : lambda x: ("Last 6 of VIN", hexlist2int(x)),
    #HSV stuff
    0xA0 : lambda x: ("HSV Shutdown txt", hsv_type[x[0]]),
    0x9F : lambda x: ("HSV Serial Number", hexlist2int(x[0])),
    0xE6 : lambda x: ("HSV Custom text", hexlist2str(x)),
    0xCF : lambda x: zip(["Cold shift light", "1st gear shiftlight",
                            "2nd gear shiftlight", "3rd gear shiftlight",
                            "4th gear shiftlight", "5th gear shiftlight"],
                        [int(i,16)/2*100 for i in x]),
    #Fuel
    0x03 : lambda x: zip(
                    ["FuelSenderOhm1","FuelSenderOhm2","FuelSenderOhm3",
                    "FuelSenderOhm4","FuelSenderOhm5","FuelSenderOhm6",
                    "FuelSenderOhm7","FuelSenderOhm8","FuelSenderOhm9",
                    "FuelSenderOhm10"], [int(x[i],16) for i in range(1,len(x),2)]),
    0x0D : lambda x: zip(
                    ["FuelSenderVol1","FuelSenderVol2","FuelSenderVol3",
                    "FuelSenderVol4","FuelSenderVol5","FuelSenderVol6",
                    "FuelSenderVol7","FuelSenderVol8","FuelSenderVol9",
                    "FuelSenderVol10"], [int(i,16)/2.0 for i in x]),
    0x80 : lambda x: zip(
                    ["Gauge-E", "Gauge-1/4", "Gauge-1/2", 
                        "Gauge-3/4","Gauge-F"], [int(i,16)/2.0 for i in x]),
    0x17 : lambda x: ("Petrol Tank capacity(L)", hexlist2int(x)/2.0),
    0x18 : lambda x: ("Petrol Tank damping const.", hexlist2int(x)),
    0x1C : lambda x: ("Low Petrol Warn", hexlist2int(x)),
    0x1D : lambda x: ("Very Low Petrol Warn", hexlist2int(x)),
}
