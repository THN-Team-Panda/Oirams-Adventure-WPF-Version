﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Transactions;
using System.Diagnostics;

namespace GameEngine
{
    public class ViewPort
    {
        /// <summary>
        /// Instance of the view canvas
        /// </summary>
        private readonly Canvas view;

        /// <summary>
        /// Instance of the map canvas
        /// Note: the map must be inside the view canvas
        /// </summary>
        private readonly Canvas map;

        /// <summary>
        /// A readonly with the view limit for the top max
        /// </summary>
        private readonly double mapLimitTopMax;

        /// <summary>
        /// A readonly with the view limit for the top min
        /// </summary>
        private readonly double mapLimitTopMin;

        /// <summary>
        /// A readonly with the view limit for the left max
        /// </summary>
        private readonly double mapLimitLeftMax;

        /// <summary>
        /// A readonly with the view limit for the left min
        /// </summary>
        private readonly double mapLimitLeftMin;

        /// <summary>
        /// The horizontal free movement zone describes the horizontal border before the camera starts to move
        /// Note: Value starts from the left hand side 
        /// </summary>
        private double horizontalFreeMovementZone = 0.45;

        /// <summary>
        /// The vertical free movement zone top describes the vertical top border before the camera starts to move
        /// Note: Value starts from the top side 
        /// </summary>
        private double verticalFreeMovementZoneTop = 0.35;

        /// <summary>
        /// The vertical free movement zone bottom describes the vertical bottom border before the camera starts to move
        /// Note: Value starts from the top side 
        /// </summary>
        private double verticalFreeMovementZoneBottom = 0.9;

        /// <summary>
        /// The Camera will try to align the camera at this point
        /// Note: Start on the top edge
        /// </summary>
        private double verticalFocusAngel = 0.8;

        /// <summary>
        /// The Camera will try to align the camera at this point
        /// Note: Start on the left edge
        /// </summary>
        private double horizontalFocusAngel = 0.35;

        /// <summary>
        /// Method to get vertical the map position. Sets the vertical map position
        /// Note: Set the map means, set the camera
        /// </summary>
        public double CurrentAngelVertical
        {
            get
            {
                return Canvas.GetTop(map);
            }
            set
            {
                double newValue = value;

                // Test the boundary
                if (newValue > mapLimitTopMin)
                    newValue = mapLimitTopMin;
                else if (newValue < mapLimitTopMax)
                    newValue = mapLimitTopMax;

                Canvas.SetTop(map, newValue);
            }
        }

        /// <summary>
        /// Method to get horizontal the map position. Sets the horizontal map position
        /// Note: Set the map means, set the camera
        /// </summary>
        public double CurrentAngelHorizontal
        {
            get
            {
                return Canvas.GetLeft(map);
            }
            set
            {
                double newValue = value;

                // Test the boundary
                if (newValue > mapLimitLeftMin)
                    newValue = mapLimitLeftMin;
                else if (newValue < mapLimitLeftMax)
                    newValue = mapLimitLeftMax;

                Canvas.SetLeft(map, newValue);
            }
        }

        /// <summary>
        /// Method to get/set the verticalFocusAngel
        /// Note: Value between 0 and 1
        /// </summary>
        public double VerticalFocusAngel
        {
            get
            {
                return verticalFocusAngel;
            }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("Value must be between 0 and 1!");

                verticalFocusAngel = value;
            }
        }

        /// <summary>
        /// Method to get/set the horizontalFocusAngel
        /// Note: Value between 0 and 1
        /// </summary>
        public double HorizontalFocusAngel
        {
            get
            {
                return horizontalFocusAngel;
            }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("Value must be between 0 and 1!");

                horizontalFocusAngel = value;
            }
        }

        /// <summary>
        /// Method to get/set the horizontalFreeMovementZone
        /// Note: Value between 0 and 1
        /// </summary>
        public double HorizontalFreeMovementZone
        {
            get
            {
                return horizontalFreeMovementZone;
            }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("Value must be between 0 and 1!");

                horizontalFreeMovementZone = value;
            }
        }

        /// <summary>
        /// Method to get/set the verticalFreeMovementZoneTop
        /// Note: Value between 0 and 1
        /// </summary>
        public double VerticalFreeMovementZoneTop
        {
            get
            {
                return verticalFreeMovementZoneTop;
            }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("Value must be between 0 and 1!");

                verticalFreeMovementZoneTop = value;
            }
        }

        /// <summary>
        /// Method to get/set the verticalFreeMovementZoneBottom
        /// Note: Value between 0 and 1
        /// </summary>
        public double VerticalFreeMovementZoneBottom
        {
            get
            {
                return verticalFreeMovementZoneBottom;
            }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("Value must be between 0 and 1!");

                verticalFreeMovementZoneBottom = value;
            }
        }

        /// <summary>
        /// Helper Method for the view width
        /// </summary>
        public double ViewWidth
        {
            get { return view.Width; }
            private set { view.Width = value; }
        }

        /// <summary>
        /// Helper Method for the view height
        /// </summary>
        public double ViewHeight
        {
            get { return view.Height; }
            private set { view.Height = value; }
        }

        /// <summary>
        /// Construct the view port
        /// Note: the map object must be a child of the view object
        /// </summary>
        /// <param name="view">view canvas with a init size</param>
        /// <param name="map">map canvas with a init size</param>
        /// <param name="initPosition">position where the camera starts</param>
        /// <exception cref="ArgumentException">Map must be a child element of view!</exception>
        public ViewPort(Canvas view, Canvas map, Point initPosition)
        {
            if (!view.Children.Contains(map))
                throw new ArgumentException("Map must be a child element of view!");

            this.view = view;
            this.map = map;

            // Set the limits!
            mapLimitLeftMin = 0;
            mapLimitLeftMax = -(this.map.Width - ViewWidth);
            mapLimitTopMin = 0;
            mapLimitTopMax = -(this.map.Height - ViewHeight);

            // Move camera
            Camera(initPosition);
        }

        /// <summary>
        /// Moves the camera to the given position
        /// Note: Without calculation the best movement
        /// </summary>
        /// <param name="position">Point with the new position</param>
        public void Camera(Point position)
        {
            CurrentAngelHorizontal = -(position.X - ViewWidth * horizontalFocusAngel);
            CurrentAngelVertical = -(position.Y - ViewHeight * VerticalFocusAngel);
        }

        /// <summary>
        /// Moves the camera to the given position without using the focus angel
        /// Note: Without calculation the best movement
        /// </summary>
        /// <param name="position">Point with the new position</param>
        public void CameraCenter(Point position)
        {
            CurrentAngelHorizontal = -(position.X - ViewWidth / 2);
            CurrentAngelVertical = -(position.Y - ViewHeight / 2);
        }

        /// <summary>
        /// Moves the camera in the given vector without using the focus angel
        /// Note: Without calculation the best movement
        /// </summary>
        /// <param name="position">Point with the new position</param>
        public void Camera(Vector position)
        {
            CurrentAngelHorizontal -= position.X;
            CurrentAngelVertical -= position.Y;
        }

        /// <summary>
        /// Moves the camera to the given position
        /// Note: Using a calculation for the best camera angel
        /// </summary>
        /// <param name="position">Point with the new position</param>
        public void SmartCamera(Point position)
        {
            Vector movingVector = CalculateAngel(position);

            // Apply only if there is a change
            if (new Vector(0, 0) != movingVector)
                Camera(movingVector);
        }

        /// <summary>
        /// Calculate the best next point by using the last applyed points
        /// </summary>
        /// <param name="applyedMovement">Current position</param>
        /// <returns>Calculated new Vector</returns>
        private Vector CalculateAngel(Point applyedMovement)
        {
            double newX = 0, newY = 0;

            // Testing the horizontal right border
            // If the point hits the horizontal border, move the point
            double horizontalBorder = -1 * ViewWidth * HorizontalFreeMovementZone + CurrentAngelHorizontal;

            if (-applyedMovement.X < horizontalBorder)
                newX = applyedMovement.X + horizontalBorder;

            // Testing the vertical border
            // If the point hits the vertical borders, move the point
            double verticalBorderTop = -1 * ViewHeight * VerticalFreeMovementZoneTop + CurrentAngelVertical;
            double verticalBorderBottom = -1 * ViewHeight * VerticalFreeMovementZoneBottom + CurrentAngelVertical;

            if (-applyedMovement.Y > verticalBorderTop)
                newY = applyedMovement.Y + verticalBorderTop;
            else if(-applyedMovement.Y < verticalBorderBottom)
             newY = applyedMovement.Y + verticalBorderBottom;

            // Return a changed vector
            return new Vector(newX, newY);
        }

    }
}