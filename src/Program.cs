using System;
using System.IO;
using NAudio.Midi;
using Newtonsoft.Json;
using SoundpadConnector;

namespace MidiBoard2SoundPad {
    public static class Program {
        private static Soundpad _soundpad;

        static int Main(string[] args)
        {
            
            _soundpad = new Soundpad();


            var config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json"));
            Console.WriteLine("The config contains the following information:");
            Console.WriteLine(JsonConvert.SerializeObject(config));
            Console.WriteLine("");
            
            var audioControl = new AudioControl(config, _soundpad);


            MidiIn midiIn =default;
            for (var device = 0; device < MidiIn.NumberOfDevices; device++) {
                Console.WriteLine(MidiIn.DeviceInfo(device).ProductName);

                if (MidiIn.DeviceInfo(device).ProductName.Contains(config.KeypadName))
                {
                    midiIn = new MidiIn(device);
                    midiIn.MessageReceived += audioControl.OnButtonPress;
                    midiIn.ErrorReceived += MidiInErrorReceived;
                    midiIn.Start();
                    
                    break;
                }
            }

            if (midiIn == null)
            {
                Console.Error.WriteLine("Device with name pad not found.");
                return -1;
            }

            _soundpad.StatusChanged += audioControl.SoundpadOnStatusChanged;
            _soundpad.ConnectAsync();
            
            
            
            //if (OnButtonPress() == 41)
            /*{
                Soundpad.PlaySound(index: 6);
            }*/
            Console.CancelKeyPress += delegate
            {
                midiIn?.Dispose();
            };
            Console.ReadLine();
            midiIn.Dispose();
            return 0;
        }

        private static void MidiInErrorReceived(object? sender, MidiInMessageEventArgs e)
        {
            Console.Error.WriteLine(e + "");
        }
    }
}