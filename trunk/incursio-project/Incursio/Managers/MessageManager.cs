using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Interface;
using Incursio.Utils;
using Incursio.Commands;

namespace Incursio.Managers
{
    class MessageManager
    {
        public static MessageManager instance;

        private SpriteFont font;
        private List<GameEvent> messages;

        private const int MESSAGE_PAUSE_TIME = 3;
        private int timeDisplayedFirst;
        private int timeDisplayedSecond;
        private int timeDisplayedThird;


        public MessageManager()
        {
            messages = new List<GameEvent>();
            timeDisplayedFirst = 0;
            timeDisplayedSecond = 0;
            timeDisplayedThird = 0;
        }

        public static MessageManager getInstance()
        {
            if (instance == null)
                instance = new MessageManager();
            return instance;
        }

        public void setFont(SpriteFont incFont)
        {
            font = incFont;
        }

        public void addMessage(GameEvent msg)
        {
            if (msg != null)
            {
                messages.Add(msg);
            }
        }

        public void update()
        {
            //if we have messages to display
            if (messages.Count > 0)
            {
                //if we've displayed the message for the specified amount of time
                if (timeDisplayedFirst >= MESSAGE_PAUSE_TIME * 60)
                {
                    messages.Remove(messages[0]);   //remove the message
                    if (messages.Count > 1)
                    {
                        timeDisplayedFirst = timeDisplayedSecond;
                        timeDisplayedSecond = 0;
                        if (messages.Count > 2)
                        {
                            timeDisplayedSecond = timeDisplayedThird;
                            timeDisplayedThird = 0;
                        }
                    }
                    else
                    {
                        timeDisplayedFirst = 0;
                    }
                    
                }
                else
                {
                    timeDisplayedFirst++;
                    if (messages.Count > 1)
                    {
                        timeDisplayedSecond++;
                    }

                    if (messages.Count > 2)
                    {
                        timeDisplayedThird++;
                    }
                }
            }
        }

        public void displayMessages(SpriteBatch spriteBatch)
        {
            if (messages.Count == 1)
            {
                spriteBatch.DrawString(font, messages[0].stringMessage, new Vector2(512, 100), Color.Red, 0, font.MeasureString(messages[0].stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
            }
            else if (messages.Count == 2)
            {
                spriteBatch.DrawString(font, messages[0].stringMessage, new Vector2(512, 100), Color.Red, 0, font.MeasureString(messages[0].stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(font, messages[1].stringMessage, new Vector2(512, 120), Color.Red, 0, font.MeasureString(messages[1].stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
            }
            else if (messages.Count == 3)
            {
                spriteBatch.DrawString(font, messages[0].stringMessage, new Vector2(512, 100), Color.Red, 0, font.MeasureString(messages[0].stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(font, messages[1].stringMessage, new Vector2(512, 120), Color.Red, 0, font.MeasureString(messages[1].stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(font, messages[2].stringMessage, new Vector2(512, 140), Color.Red, 0, font.MeasureString(messages[2].stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
            }
        }

        public Coordinate getLastMessageLocation(){
            if (messages.Count > 0)
                return messages[messages.Count - 1].location;

            return null;
        }
    }
}
