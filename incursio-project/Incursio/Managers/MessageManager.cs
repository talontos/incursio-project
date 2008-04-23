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
        private const int MAXIMUM_DISPLAYED_SYSTEM_MESSAGES = 3;
        private int numberSystemMessages = 0;
        private int systemMessageYCoordOffset = 0;



        public MessageManager()
        {
            messages = new List<GameEvent>();
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

                messages.ForEach(delegate(GameEvent e)
                {
                    switch(e.type)
                    {
                        case State.EventType.TAKING_DAMAGE:
                        case State.EventType.GAIN_RESOURCE:
                            if (e.displayTick > MESSAGE_PAUSE_TIME * 20)
                            {
                                messages.Remove(e);
                            }
                            break;
                        case State.EventType.CANT_MOVE_THERE:
                        case State.EventType.CREATION_COMPLETE:
                        case State.EventType.NOT_ENOUGH_RESOURCES:
                        case State.EventType.UNDER_ATTACK:
                            if (e.displayTick > MESSAGE_PAUSE_TIME * 60)
                            {
                                messages.Remove(e);
                            }
                            break;
                    }
                    
                });
                //if we've displayed the message for the specified amount of time
                /*if (timeDisplayedFirst >= MESSAGE_PAUSE_TIME * 60)
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
                }*/
            }
        }
        public void displayMessages(SpriteBatch spriteBatch)
        {
            Coordinate loc = new Coordinate();

            messages.ForEach(delegate(GameEvent e)
            {
                switch (e.type)
                {
                    case State.EventType.CANT_MOVE_THERE:
                    case State.EventType.CREATION_COMPLETE:
                    case State.EventType.NOT_ENOUGH_RESOURCES:
                    case State.EventType.UNDER_ATTACK:
                        if (numberSystemMessages <= MAXIMUM_DISPLAYED_SYSTEM_MESSAGES)
                        {
                            e.displayTick++;
                            spriteBatch.DrawString(font, e.stringMessage, new Vector2(75, 600 + systemMessageYCoordOffset), Color.Red, 0, font.MeasureString(e.stringMessage) / 4, 1.0f, SpriteEffects.None, 0.5f);
                            systemMessageYCoordOffset = systemMessageYCoordOffset + 15;
                            numberSystemMessages++;
                        }
                        
                        break;

                    case State.EventType.TAKING_DAMAGE:
                        e.displayTick++;
                        if(Incursio.getInstance().currentMap.isOnScreen(e.location))
                        {
                            loc = Incursio.getInstance().currentMap.positionOnScreen(e.location);
                            spriteBatch.DrawString(font, e.stringMessage, new Vector2(loc.x, loc.y - 35), Color.Red, 0, font.MeasureString(e.stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
                        }
                        break;

                    case State.EventType.GAIN_RESOURCE:
                        e.displayTick++;
                        if (Incursio.getInstance().currentMap.isOnScreen(e.location))
                        {
                            loc = Incursio.getInstance().currentMap.positionOnScreen(e.location);
                            spriteBatch.DrawString(font, e.stringMessage, new Vector2(loc.x, loc.y - 35), Color.Gold, 0, font.MeasureString(e.stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
                        }  
                        break;
                }
            });

            //reset each time
            numberSystemMessages = 0;
            systemMessageYCoordOffset = 0;
            
        }

        public Coordinate getLastMessageLocation(){
            if (messages.Count > 0)
                return messages[messages.Count - 1].location;

            return null;
        }
    }
}
