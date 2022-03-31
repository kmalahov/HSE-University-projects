using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WF.coursework
{
    class DataClasses
    {

    }

    public class Header
    {
        private DateTime _time;

        private string _headerTextInsteadOfTime;
        private string _headerText;
        private int _startLocation;

        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }

        public int StartLocation
        {
            get { return _startLocation; }
            set { _startLocation = value; }
        }

        public string HeaderTextInsteadOfTime
        {
            get { return _headerTextInsteadOfTime; }
            set { _headerTextInsteadOfTime = value; }
        }

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
    }

    public class ChartBarDate
    {
        private DateTime _startValue;
        private DateTime _endValue;

        private Color _color;
        private Color _hoverColor;

        private Location _topLocation = new Location();
        private Location _bottomLocation = new Location();

        private object _value;
        private string _text;
        private int _rowIndex;
        private bool _hideFromMouseMove = false;

        internal class Location
        {
            private Point _right = new Point(0, 0);
            private Point _left = new Point(0, 0);

            public Point Right
            {
                get { return _right; }
                set { _right = value; }
            }

            public Point Left
            {
                get { return _left; }
                set { _left = value; }
            }
        }

        public DateTime StartValue
        {
            get { return _startValue; }
            set { _startValue = value; }
        }

        public DateTime EndValue
        {
            get { return _endValue; }
            set { _endValue = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Color HoverColor
        {
            get { return _hoverColor; }
            set { _hoverColor = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        public bool HideFromMouseMove
        {
            get { return _hideFromMouseMove; }
            set { _hideFromMouseMove = value; }
        }

        internal Location TopLocation
        {
            get { return _topLocation; }
            set { _topLocation = value; }
        }

        internal Location BottomLocation
        {
            get { return _bottomLocation; }
            set { _bottomLocation = value; }
        }
    }

    public class BarInformation
    {
        private string _rowText;
        private DateTime _fromTime;
        private DateTime _toTime;
        private Color _color;
        private Color _hoverColor;
        private int _index;

        public string RowText
        {
            get { return _rowText; }
            set { _rowText = value; }
        }

        public DateTime FromTime
        {
            get { return _fromTime; }
            set { _fromTime = value; }
        }

        public DateTime ToTime
        {
            get { return _toTime; }
            set { _toTime = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Color HoverColor
        {
            get { return _hoverColor; }
            set { _hoverColor = value; }
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public BarInformation()
        {

        }

        public BarInformation(string rowText, DateTime fromTime, DateTime totime, Color color, Color hoverColor, int index)
        {
            RowText = rowText;
            FromTime = fromTime;
            ToTime = totime;
            Color = color;
            HoverColor = hoverColor;
            Index = index;
        }
    }
}
