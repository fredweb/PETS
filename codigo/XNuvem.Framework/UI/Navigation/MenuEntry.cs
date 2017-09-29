/****************************************************************************************
 *
 * Autor: George Santos
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace XNuvem.UI.Navigation
{
    public enum MenuType
    {
        String,
        Group,
        Separator
    }
    public class MenuEntry
    {
        public string Permission { get; set; }
        public string Icon { get; set; }
        public MenuType Type { get; set; }
        public string Position { get; set; }
        public int Level { 
            get { 
                return GetLevelFromPosition(); 
            } 
        }

        public string Father {
            get {
                return GetFatherFromPosition();
            }
        }

        public int Order { get; set; }
        public string Title { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
        public IList<MenuEntry> Submenu { get; private set; }
        public string ImageUrl { get; set; }

        public MenuEntry() {
            Submenu = new List<MenuEntry>();
        }

        internal int GetLevelFromPosition() {
            var strLastPosition = string.IsNullOrEmpty(Position) ? Position : Position.Split('.').Reverse().FirstOrDefault();
            int result = Int32.MaxValue;
            if (!Int32.TryParse(strLastPosition, out result))
                return Int32.MaxValue;
            return result;
        }

        internal string GetFatherFromPosition() {
            return string.IsNullOrEmpty(Position) || !Position.Contains(".") ? "" : Position.Substring(0, Position.LastIndexOf('.'));
        }

        /// <summary>
        /// Return a list of MenuEntry ordered by position
        /// </summary>
        /// <returns>A list of MenuEntry</returns>
        public IEnumerable<MenuEntry> Transverse(bool includeRoot = false) {
            var stack = new Stack<MenuEntry>();
            if (includeRoot) {
                stack.Push(this);
            }
            else {
                var roots = this.Submenu.OrderByDescending(m => m.Level).ToList();
                foreach (var item in roots) {
                    stack.Push(item);
                }
            }
            while (stack.Any()) {
                var next = stack.Pop();
                yield return next;
                var ordered = next.Submenu.OrderByDescending(m => m.Level).ToList();
                foreach (var child in ordered) {
                    stack.Push(child);
                }
            }
        }

        public IEnumerable<MenuEntry> SubmenuOrdered {
            get {
                return Submenu.OrderBy(m => m.Order).ThenBy(m => m.Level).ToList();
            }
        }
    }
}
