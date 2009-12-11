//Date: 5/7/2009
using System;
using System.Drawing;
using System.Timers;
using Tools.GenericUtilities;

namespace Tools.GenericRanges
{
    /// <summary>
    /// A range that contains two colors which will alternatively be assigned to the current color by a tmrBlink
    /// Any listener to the CurrentColor Changed Event will be informed of the change and can
    /// retrieve the new current color from the EventArgs
    /// </summary>
    public class BlinkingColorCodedRange<T> : ColorCodedRange<T>, IColorCodedRange<T> where T : IComparable
    {
		
        public BlinkingColorCodedRange(T minValue, T maxValue, Color colorCode, Color alternateColor)
            : this(minValue,  maxValue, colorCode, alternateColor, 1000) {}
		                               
        public BlinkingColorCodedRange(T minValue, T maxValue, Color colorCode, Color alternateColor, double interval)
            : base(minValue, maxValue, colorCode) 
        {
            this.AlternateColor = alternateColor;
            CreateTimer(interval);
        }
        private Timer tmrBlink;
		
        #region Properties

        public Color AlternateColor {get; protected set;}
		
        private Color currentColor;
		
        protected Color CurrentColor{
            get { return currentColor; }
            set { 
                currentColor = value;
                OnCurrentColorChanged(new GenericEventArgs<Color>(CurrentColor));
            }
        }
		
        #endregion
		
        #region Events

        EventHandler<GenericEventArgs<Color>> currentColorChanged;
        public event EventHandler<GenericEventArgs<Color>> CurrentColorChanged
        {
            add {
                if (TheDelegateIsNotInTheInvocationListYet(value, currentColorChanged))
                {
                    this.currentColorChanged += value;
                    this.tmrBlink.Start();
                }
            }
            remove {
                this.currentColorChanged -= value;
                if(this.currentColorChanged == null)
                {
                    tmrBlink.Stop();
                }
            }
        }
		
        #endregion
		
        #region OnCurrentColorChanged

        protected virtual void OnCurrentColorChanged(GenericEventArgs<Color> e)
        {
            EventHandler<GenericEventArgs<Color>> handler = currentColorChanged;
			
            if (handler != null)
            {
                Delegate[] eventHandlers = handler.GetInvocationList();
                foreach (Delegate currentHandler in eventHandlers)
                {
                    EventHandler<GenericEventArgs<Color>> currentListener =
                        currentHandler as EventHandler<GenericEventArgs<Color>>;
					
                    if(currentListener != null)
                        currentListener(this, e);
                }
            }
        }
		
        #endregion
		
        #region CreateTimer

        void CreateTimer(double interval)
        {
            tmrBlink = new Timer(interval);
			
            tmrBlink.Elapsed += delegate
                                {
                                    CurrentColor = (CurrentColor == AlternateColor) ? ColorCode : AlternateColor;
                                };
        }
		
        #endregion
		
		
        #region ClearTheInvocationListOf

        static void ClearTheInvocationListOf<K> (EventHandler<K> handler) where K : EventArgs
        {
            if (handler != null)
            {
                Delegate[] eventHandlers = handler.GetInvocationList();
				
                foreach (Delegate currentHandler in eventHandlers)
                {
                    EventHandler<K> currentListener = (EventHandler<K>)currentHandler;
                    handler -= currentListener;
                }
            }
        }
		
        #endregion
		
        #region TheDelegateIsNotInTheInvocationListYet
		
        static bool TheDelegateIsNotInTheInvocationListYet<K> (Delegate value, EventHandler<K> handler) where K : EventArgs
        {
            if (handler != null)
            {
                Delegate[] eventHandlers = handler.GetInvocationList();
				
                foreach (Delegate currentHandler in eventHandlers)
                {
                    EventHandler<K> currentListener = 
                        (EventHandler<K>)currentHandler;
                    if(currentHandler.Equals(value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
		
        #endregion
		
		
		
		
    }
}