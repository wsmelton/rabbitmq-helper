﻿using System;
using System.Linq;
using System.Text;

namespace Thycotic.CLI.Commands
{
    public abstract class CommandBase : ICommand
    {

        public virtual string Name { get; set; }
        public virtual string Area { get; set; }
        public virtual string[] Aliases { get; set; }
        public virtual string Description { get; set; }

        protected bool Equals(CommandBase other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Area, other.Area) && Equals(Aliases, other.Aliases);
        }

        public static bool operator ==(CommandBase left, CommandBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CommandBase left, CommandBase right)
        {
            return !Equals(left, right);
        }

        public Func<ConsoleCommandParameters, int> Action { get; set; }

        protected CommandBase()
        {
            Action = parameters => 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Area != null ? Area.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Aliases != null ? Aliases.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CommandBase) obj);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Name);

            if (Aliases != null && Aliases.Any())
            {
                sb.Append(string.Format(" ({0})", string.Join(" ",Aliases)));
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                sb.Append(string.Format(" - {0}", Description));
            }

            return sb.ToString();
        }
    }
}