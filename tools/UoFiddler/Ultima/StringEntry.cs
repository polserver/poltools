using System;

namespace Ultima
{
	public sealed class StringEntry
	{
        [Flags]
        public enum CliLocFlag
        {
            Original = 0x0,
            Custom = 0x1,
            Modified = 0x2
        }

		private int m_Number;
		private string m_Text;
        private CliLocFlag m_Flag;

        public int Number { get { return m_Number; } }
        public string Text
        {
            get { return m_Text; }
            set
            {
                if (value == null)
                    m_Text = "";
                else
                    m_Text = value;
            }
        }
        public CliLocFlag Flag { get { return m_Flag; } set { m_Flag = value; } }

		public StringEntry( int number, string text, byte flag )
		{
			m_Number = number;
			m_Text = text;
            m_Flag = (CliLocFlag)flag;
		}

        public StringEntry(int number, string text, CliLocFlag flag)
        {
            m_Number = number;
            m_Text = text;
            m_Flag = flag;
        }
	}
}