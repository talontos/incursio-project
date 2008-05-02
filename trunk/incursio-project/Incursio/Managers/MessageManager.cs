/****************************************
 * Copyright © 2008, Team RobotNinja:
 * 
 *     - Henry Armstrong
 *     - Andy Burras
 *     - Mitch Martin
 *     - Xuan Yu
 * 
 * All Rights Reserved
 ***************************************/

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
        private static MessageManager instance;

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

        public void reinitializeInstance(){
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
                SoundManager.getInstance().PlaySound(msg.audibleMessage, false);
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
                        case State.EventType.HEALING:
                            if (e.displayTick > MESSAGE_PAUSE_TIME * 20)
                            {
                                messages.Remove(e);
                            }
                            break;
                        case State.EventType.LEVEL_UP:
                        case State.EventType.ENEMY_CAPTURING_POINT:
                        case State.EventType.POINT_CAPTURED:
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
                
            }
        }
        public void displayMessages(SpriteBatch spriteBatch)
        {
            Coordinate loc = new Coordinate();

            messages.ForEach(delegate(GameEvent e)
            {
                switch (e.type)
                {
                    case State.EventType.LEVEL_UP:
                    case State.EventType.ENEMY_CAPTURING_POINT:
                    case State.EventType.POINT_CAPTURED:
                    case State.EventType.CANT_MOVE_THERE:
                    case State.EventType.CREATION_COMPLETE:
                    case State.EventType.NOT_ENOUGH_RESOURCES:
                    case State.EventType.UNDER_ATTACK:
                        if (numberSystemMessages <= MAXIMUM_DISPLAYED_SYSTEM_MESSAGES)
                        {
                            e.displayTick++;
                            spriteBatch.DrawString(font, e.stringMessage, new Vector2(75, 620 + systemMessageYCoordOffset), Color.Red, 0, font.MeasureString(e.stringMessage) / 4, 1.0f, SpriteEffects.None, 0.5f);
                            systemMessageYCoordOffset = systemMessageYCoordOffset + 20;
                            numberSystemMessages++;
                        }
                        
                        break;

                    case State.EventType.TAKING_DAMAGE:
                        e.displayTick++;
                        if (MapManager.getInstance().currentMap.isOnScreen(e.location))
                        {
                            loc = MapManager.getInstance().currentMap.positionOnScreen(e.location);
                            spriteBatch.DrawString(font, e.stringMessage, new Vector2(loc.x, loc.y - 35), Color.Red, 0, font.MeasureString(e.stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
                        }
                        break;

                    case State.EventType.GAIN_RESOURCE:
                        e.displayTick++;
                        if (MapManager.getInstance().currentMap.isOnScreen(e.location))
                        {
                            loc = MapManager.getInstance().currentMap.positionOnScreen(e.location);
                            spriteBatch.DrawString(font, e.stringMessage, new Vector2(loc.x, loc.y - 35), Color.Gold, 0, font.MeasureString(e.stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
                        }  
                        break;

                    case State.EventType.HEALING:
                        e.displayTick++;
                        if (MapManager.getInstance().currentMap.isOnScreen(e.location))
                        {
                            loc = MapManager.getInstance().currentMap.positionOnScreen(e.location);
                            spriteBatch.DrawString(font, e.stringMessage, new Vector2(loc.x, loc.y - 35), Color.Blue, 0, font.MeasureString(e.stringMessage) / 2, 1.0f, SpriteEffects.None, 0.5f);
                        }
                        break;

                    case State.EventType.GAME_OVER_MAN:
                        spriteBatch.DrawString(font, e.stringMessage, new Vector2(500, 300 + systemMessageYCoordOffset), Color.Red, 0, font.MeasureString(e.stringMessage) / 4, 1.0f, SpriteEffects.None, 0.5f);
                        numberSystemMessages++;
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
