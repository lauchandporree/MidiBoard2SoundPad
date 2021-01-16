using System;
using NAudio.Midi;
using SoundpadConnector;

namespace MidiBoard2SoundPad {
    public static class Program {
        public static Soundpad Soundpad;

        static void Main(string[] args) {
            for (int device = 0; device < MidiIn.NumberOfDevices; device++) {
                Console.WriteLine(MidiIn.DeviceInfo(device).ProductName);
            }
            var midiIn = new MidiIn(0);
            midiIn.MessageReceived += midiIn_MessageReceived;
            midiIn.ErrorReceived += midiIn_ErrorReceived;
            midiIn.Start();
            Soundpad = new Soundpad();
            //Soundpad.StatusChanged += SoundpadOnStatusChanged;

            Soundpad.ConnectAsync();

            Console.ReadLine();
        }

        private static void midiIn_ErrorReceived(object? sender, MidiInMessageEventArgs e)
        {
            Console.Error.WriteLine(e + "");
        }

        private static void midiIn_MessageReceived(object? sender, MidiInMessageEventArgs e)
        {
            //Console.WriteLine($"{e.Timestamp}: {e.MidiEvent}: {e.RawMessage}");
            var controlChangeEvent = ((ControlChangeEvent) e.MidiEvent);
            if (controlChangeEvent.ControllerValue == 127)
            {
                OnButtonPress(controlChangeEvent.Controller);
            } 
        }

        private static void OnButtonPress(MidiController id)
        {
            Console.WriteLine("Button press: " + id);
        }
        
        private static void SoundpadOnStatusChanged(object sender, EventArgs e) {
            Console.WriteLine(Soundpad.ConnectionStatus);

            if (Soundpad.ConnectionStatus == ConnectionStatus.Connected) {
                Soundpad.PlaySound(1);
            }
        }
    }
}