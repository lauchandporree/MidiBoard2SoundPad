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

            Soundpad = new Soundpad();
            Soundpad.StatusChanged += SoundpadOnStatusChanged;

            Soundpad.ConnectAsync();

            Console.ReadLine();
        }

        private static void SoundpadOnStatusChanged(object sender, EventArgs e) {
            Console.WriteLine(Soundpad.ConnectionStatus);

            if (Soundpad.ConnectionStatus == ConnectionStatus.Connected) {
                Soundpad.PlaySound(1);
            }
        }
    }
}