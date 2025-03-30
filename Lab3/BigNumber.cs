using System;
using System.Collections.Generic;
using System.Linq;

namespace Laba3
{
    public class BigNumber : IComparable<BigNumber>
    {
        private List<int> digits = new List<int>();
        private BigNumber() { }
        public BigNumber(int value)
        {
            if (value == 0)
            {
                digits.Add(0);
                return;
            }

            int temp = Math.Abs(value);
            while (temp > 0)
            {
                digits.Add(temp % 10);
                temp /= 10;
            }
        }

        public BigNumber(string strValue)
        {
            strValue = strValue.TrimStart('0');

            if (string.IsNullOrEmpty(strValue))
            {
                digits.Add(0);
                return;
            }

            for (int i = strValue.Length - 1; i >= 0; i--)
            {
                digits.Add((int)(strValue[i] - '0'));
            }
        }
        public override string ToString()
        {
            var arr = digits.ToArray();
            Array.Reverse(arr);

            string result = string.Join("", arr).TrimStart('0');
            return string.IsNullOrEmpty(result) ? "0" : result;
        }

        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            var result = new BigNumber();
            result.digits.Clear();

            int carry = 0;
            int maxLength = Math.Max(a.digits.Count, b.digits.Count);

            for (int i = 0; i < maxLength || carry != 0; i++)
            {
                int x = (i < a.digits.Count) ? a.digits[i] : 0;
                int y = (i < b.digits.Count) ? b.digits[i] : 0;

                int sum = x + y + carry;
                carry = sum / 10;
                result.digits.Add(sum % 10);
            }
            return result;
        }
        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            var result = new BigNumber();
            result.digits.Clear();

            int carry = 0;
            for (int i = 0; i < a.digits.Count; i++)
            {
                int x = a.digits[i];
                int y = (i < b.digits.Count) ? b.digits[i] : 0;
                int diff = x - y - carry;

                if (diff < 0)
                {
                    diff += 10;
                    carry = 1;
                }
                else
                {
                    carry = 0;
                }
                result.digits.Add(diff);
            }

            while (result.digits.Count > 1 && result.digits[^1] == 0)
            {
                result.digits.RemoveAt(result.digits.Count - 1);
            }
            return result;
        }
        public int CompareTo(BigNumber other)
        {
            if (digits.Count > other.digits.Count) return 1;
            if (digits.Count < other.digits.Count) return -1;

            for (int i = digits.Count - 1; i >= 0; i--)
            {
                if (digits[i] > other.digits[i]) return 1;
                if (digits[i] < other.digits[i]) return -1;
            }
            return 0;
        }

        public static bool operator >(BigNumber a, BigNumber b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(BigNumber a, BigNumber b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >=(BigNumber a, BigNumber b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static bool operator <=(BigNumber a, BigNumber b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator ==(BigNumber a, BigNumber b)
        {
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(BigNumber a, BigNumber b)
        {
            return a.CompareTo(b) != 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is BigNumber bn)
                return this.CompareTo(bn) == 0;
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
