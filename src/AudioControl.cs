using System;
using NAudio.Midi;
using SoundpadConnector;

namespace MidiBoard2SoundPad
{
    public class AudioControl
    {
        private readonly Configuration _config;
        private readonly Soundpad _soundpad;
        private bool _soundpadConnected;

        public AudioControl(Configuration config, Soundpad soundpad)
        {
            _config = config;
            _soundpad = soundpad;
        }

        public void OnButtonPress(object? sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine($"{e.Timestamp}: {e.MidiEvent}: {e.RawMessage}");
            var controlChangeEvent = ((ControlChangeEvent) e.MidiEvent);
            if (controlChangeEvent.ControllerValue == 127)
            {
                OnButtonPress(controlChangeEvent.Controller);
            } 

        }

        private void OnButtonPress(MidiController id)
        {

            if (_soundpadConnected && _config.Mapping.ContainsKey((int) id))
            {
                _soundpad.PlaySound(_config.Mapping[(int) id]);
            }
            
            Console.WriteLine("Button press: " + id);
        }

        public void SoundpadOnStatusChanged(object? sender, EventArgs e)
        {
            Console.WriteLine(_soundpad.ConnectionStatus);

            if (_soundpad.ConnectionStatus == ConnectionStatus.Connected)
            {
                _soundpadConnected = true;
            }

        }
    }
}