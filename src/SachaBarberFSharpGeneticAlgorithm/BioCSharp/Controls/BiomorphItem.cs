using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using BioCSharp.Biomorphs;
using BioCSharp.Interfaces;

namespace BioCSharp.Controls
{
    public class BiomorphItem : ContentPresenter
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);


            double xSize = 150.0;
            double ySize = 150.0;

            IEvolvableSkull organism = this.DataContext as IEvolvableSkull;


            //Get center point of whole shape
            int shapeWidth = organism.GenotypeWidths[Genes.FACE_GENE] +
                organism.GenotypeWidths[Genes.LEFTBONE_TOP_GENE] + organism.GenotypeWidths[Genes.RIGHTBONE_TOP_GENE];
            int shapeHeight = organism.GenotypeHeights[Genes.FACE_GENE] + organism.GenotypeHeights[Genes.TEETH_GENE];

            //Determine some offset values
            double offsetX = (xSize - shapeWidth) / 2;
            double offsetY = (ySize - shapeHeight) / 2;

            //**************************************************************************************************************************
            //     THE ORDER IN WHICH THE GENES ARE DRAWN IS VERY IMPORTANT SO DONT MOVE ANY CODE                 
            //     ORDER MUST BE  
            //**************************************************************************************************************************



            //==================================================================================================
            //  GENE[4] = Draw left top bone
            //==================================================================================================
            // Draw left top bone
            var leftBoneTopGeneBrush = new SolidColorBrush(organism.GenotypeColours[Genes.LEFTBONE_TOP_GENE]);
            drawingContext.DrawRectangle(leftBoneTopGeneBrush,
                new Pen(leftBoneTopGeneBrush, 0.0),
                new Rect(
                    offsetX, 
                    offsetY, 
                    organism.GenotypeWidths[Genes.LEFTBONE_TOP_GENE],
                    organism.GenotypeHeights[Genes.LEFTBONE_TOP_GENE]));

            drawingContext.DrawRectangle(Brushes.White,
                new Pen(Brushes.White, 0.0),
                new Rect(
                    offsetX, 
                    (offsetY + (organism.GenotypeHeights[Genes.LEFTBONE_TOP_GENE] / 3)), 
                    (organism.GenotypeWidths[Genes.LEFTBONE_TOP_GENE] / 4), 
                    (organism.GenotypeHeights[Genes.LEFTBONE_TOP_GENE] / 3)));




            ////==================================================================================================
            ////  GENE[7] = Draw left bottom bone
            ////==================================================================================================

            //// Draw left bottom bone

            var bottomBoneOffsetY = (offsetY + (organism.GenotypeHeights[Genes.FACE_GENE] -
                organism.GenotypeHeights[Genes.LEFTBONE_BOTTOM_GENE]));
         

            var leftBoneBottomGeneBrush = new SolidColorBrush(organism.GenotypeColours[Genes.LEFTBONE_BOTTOM_GENE]);
            drawingContext.DrawRectangle(leftBoneBottomGeneBrush,
                new Pen(leftBoneBottomGeneBrush, 0.0),
                new Rect(
                    offsetX + (organism.GenotypeWidths[Genes.LEFTBONE_TOP_GENE] - 
                        organism.GenotypeWidths[Genes.LEFTBONE_BOTTOM_GENE]), 
                    bottomBoneOffsetY, 
                    organism.GenotypeWidths[Genes.LEFTBONE_BOTTOM_GENE], 
                    organism.GenotypeHeights[Genes.LEFTBONE_BOTTOM_GENE]));

            drawingContext.DrawRectangle(Brushes.White,
                new Pen(Brushes.White, 0.0),
                new Rect(
                    offsetX + (organism.GenotypeWidths[Genes.LEFTBONE_TOP_GENE] - 
                        organism.GenotypeWidths[Genes.LEFTBONE_BOTTOM_GENE]),
                    (bottomBoneOffsetY + (organism.GenotypeHeights[Genes.LEFTBONE_BOTTOM_GENE] / 3)),
                    (organism.GenotypeWidths[Genes.LEFTBONE_BOTTOM_GENE] / 4), 
                    (organism.GenotypeHeights[Genes.LEFTBONE_BOTTOM_GENE] / 3)));


   

            //==================================================================================================
            //  GENE[0] = Draw face rect
            //==================================================================================================
            offsetX += organism.GenotypeWidths[Genes.LEFTBONE_TOP_GENE];
            // store faceX for use when drawing shapes
            var faceX = offsetX;
            // store faceY for use when drawing shapes
            var faceY = offsetY + organism.GenotypeHeights[Genes.FACE_GENE];

            var faceGeneBrush = new SolidColorBrush(organism.GenotypeColours[Genes.FACE_GENE]);
            drawingContext.DrawRectangle(faceGeneBrush,
                new Pen(faceGeneBrush, 0.0),
                new Rect(
                    offsetX, 
                    offsetY, 
                    organism.GenotypeWidths[Genes.FACE_GENE], 
                    organism.GenotypeHeights[Genes.FACE_GENE]));






            //==================================================================================================
            //  GENE[5] = Draw right top bone
            //==================================================================================================
            // Draw right top bone

            offsetX += organism.GenotypeWidths[Genes.FACE_GENE];

            var rightBoneTopGeneBrush = new SolidColorBrush(organism.GenotypeColours[Genes.RIGHTBONE_TOP_GENE]);
            drawingContext.DrawRectangle(rightBoneTopGeneBrush,
                new Pen(rightBoneTopGeneBrush, 0.0),
                new Rect(
                    offsetX,
                    offsetY,
                    organism.GenotypeWidths[Genes.RIGHTBONE_TOP_GENE],
                    organism.GenotypeHeights[Genes.RIGHTBONE_TOP_GENE]));

            drawingContext.DrawRectangle(Brushes.White,
                new Pen(Brushes.White, 0.0),
                new Rect(
                    offsetX + organism.GenotypeWidths[Genes.RIGHTBONE_TOP_GENE] - (organism.GenotypeWidths[Genes.RIGHTBONE_TOP_GENE] / 4),
                    (offsetY + (organism.GenotypeHeights[Genes.RIGHTBONE_TOP_GENE] / 3)),
                    (organism.GenotypeWidths[Genes.RIGHTBONE_TOP_GENE] / 4),
                    (organism.GenotypeHeights[Genes.RIGHTBONE_TOP_GENE] / 3)));

            ////==================================================================================================
            ////  GENE[7] = Draw right bottom bone
            ////==================================================================================================

            //// Draw right bottom bone

            bottomBoneOffsetY = (offsetY + (organism.GenotypeHeights[Genes.FACE_GENE] -
                organism.GenotypeHeights[Genes.LEFTBONE_BOTTOM_GENE]));


            var rightBoneBottomGeneBrush = new SolidColorBrush(organism.GenotypeColours[Genes.RIGHTBONE_BOTTOM_GENE]);
            drawingContext.DrawRectangle(rightBoneBottomGeneBrush,
                new Pen(leftBoneBottomGeneBrush, 0.0),
                new Rect(
                    offsetX,
                    bottomBoneOffsetY,
                    organism.GenotypeWidths[Genes.RIGHTBONE_BOTTOM_GENE],
                    organism.GenotypeHeights[Genes.RIGHTBONE_BOTTOM_GENE]));

            drawingContext.DrawRectangle(Brushes.White,
                new Pen(Brushes.White, 0.0),
                new Rect(
                    offsetX + organism.GenotypeWidths[Genes.RIGHTBONE_BOTTOM_GENE] - 
                        (organism.GenotypeWidths[Genes.RIGHTBONE_BOTTOM_GENE] / 4),
                    (bottomBoneOffsetY + (organism.GenotypeHeights[Genes.RIGHTBONE_BOTTOM_GENE] / 3)),
                    (organism.GenotypeWidths[Genes.RIGHTBONE_BOTTOM_GENE] / 4),
                    (organism.GenotypeHeights[Genes.RIGHTBONE_BOTTOM_GENE] / 3)));


            //==================================================================================================
            //  GENE[3] = Draw nose
            //==================================================================================================
            var nose_OffsetX = (faceX + (organism.GenotypeWidths[Genes.FACE_GENE] / 2)) -
                (organism.GenotypeWidths[Genes.NOSE_GENE] / 2);
            var nose_OffsetY = (faceY - (3 * organism.GenotypeHeights[Genes.NOSE_GENE]));

            // Draw jaw bone
            drawingContext.DrawRectangle(Brushes.Black,
               new Pen(Brushes.Black, 0.0),
               new Rect(
                    nose_OffsetX,
                    nose_OffsetY,
                    organism.GenotypeWidths[Genes.NOSE_GENE], organism.GenotypeHeights[Genes.NOSE_GENE]));



            //==================================================================================================
            //  GENE[1] = Draw left eye
            //==================================================================================================
            // store faceX + 1 eye width for use when drawing 1st eye
            var eyeLeftX = (xSize / 2) - (organism.GenotypeWidths[Genes.FACE_GENE] / 4);
            // store faceY + 1 eye height for use when drawing 1st eye
            var eyeLeftY = offsetY + (organism.GenotypeHeights[Genes.FACE_GENE] / 4);
            // draw the whole eye
            drawingContext.DrawEllipse(Brushes.Black,
              new Pen(Brushes.Black, 0.0),
                        new Point(eyeLeftX, eyeLeftY),
                        organism.GenotypeWidths[Genes.LEFT_EYE_GENE],
                        organism.GenotypeHeights[Genes.LEFT_EYE_GENE]);

            // recalculate X position to put pupil into eye
            eyeLeftX = (eyeLeftX + (organism.GenotypeWidths[Genes.LEFT_EYE_GENE] / 4));
            // recalculate Y position to put pupil into eye
            eyeLeftY = (eyeLeftY + (organism.GenotypeHeights[Genes.LEFT_EYE_GENE] / 4));
            // draw pupil in eye - it is 1/2 the size of the whole eye
            var leftEyePupilColourBrush = new SolidColorBrush(organism.AvailableEyeColours[Genes.LEFT_EYE_GENE]);
            drawingContext.DrawEllipse(leftEyePupilColourBrush,
              new Pen(leftEyePupilColourBrush, 0.0),
                        new Point(eyeLeftX, eyeLeftY),
                        organism.GenotypeWidths[Genes.LEFT_EYE_GENE] / 2,
                        organism.GenotypeHeights[Genes.LEFT_EYE_GENE] / 2);







            //==================================================================================================
            //  GENE[2] = Draw right eye
            //==================================================================================================
            eyeLeftX = (xSize / 2) + (organism.GenotypeWidths[Genes.FACE_GENE] / 4);
            // store faceY + 1 eye height for use when drawing 1st eye
            eyeLeftY = offsetY + (organism.GenotypeHeights[Genes.FACE_GENE] / 4);
            // draw the whole eye
            drawingContext.DrawEllipse(Brushes.Black,
              new Pen(Brushes.Black, 0.0),
                        new Point(eyeLeftX, eyeLeftY),
                        organism.GenotypeWidths[Genes.LEFT_EYE_GENE],
                        organism.GenotypeHeights[Genes.LEFT_EYE_GENE]);

            // recalculate X position to put pupil into eye
            eyeLeftX = (eyeLeftX + (organism.GenotypeWidths[Genes.RIGHT_EYE_GENE] / 4));
            // recalculate Y position to put pupil into eye
            eyeLeftY = (eyeLeftY + (organism.GenotypeHeights[Genes.RIGHT_EYE_GENE] / 4));
            // draw pupil in eye - it is 1/2 the size of the whole eye
            var rightEyePupilColourBrush = new SolidColorBrush(organism.AvailableEyeColours[Genes.LEFT_EYE_GENE]);
            drawingContext.DrawEllipse(rightEyePupilColourBrush,
              new Pen(rightEyePupilColourBrush, 0.0),
                        new Point(eyeLeftX, eyeLeftY),
                        organism.GenotypeWidths[Genes.LEFT_EYE_GENE] / 2,
                        organism.GenotypeHeights[Genes.LEFT_EYE_GENE] / 2);



            //==================================================================================================
            //  GENE[6] = Draw teeth (Actually jaw bone if one wanted to be pedantic)
            //==================================================================================================
            var jaw_OffsetX = (faceX + (organism.GenotypeWidths[Genes.FACE_GENE] / 2)) - 
                (organism.GenotypeWidths[Genes.TEETH_GENE] / 2);
            offsetY = faceY;
            // Draw jaw bone
            var teethColourBrush = new SolidColorBrush(organism.AvailableEyeColours[Genes.TEETH_GENE]);
            drawingContext.DrawRectangle(teethColourBrush,
               new Pen(teethColourBrush, 0.0),
               new Rect(
                    jaw_OffsetX,
                    offsetY, 
                    organism.GenotypeWidths[Genes.TEETH_GENE],
                    organism.GenotypeHeights[Genes.TEETH_GENE]));



            //Draw individual teeth into jaw bone - always drawn black

            // Work out spacing for teeth and store for use when drawing shapes
            var individual_tooth_OffsetX = (int)(organism.GenotypeWidths[Genes.TEETH_GENE] / 4);

            // Tooth1
            drawingContext.DrawLine(new Pen(Brushes.Black,1.0),
                new Point(jaw_OffsetX + individual_tooth_OffsetX, 
                            offsetY), 
                new Point(jaw_OffsetX + individual_tooth_OffsetX,
                            offsetY + organism.GenotypeHeights[Genes.TEETH_GENE]));

            // Tooth2 (Offset from tooth1)
            jaw_OffsetX += individual_tooth_OffsetX;
            drawingContext.DrawLine(new Pen(Brushes.Black, 1.0),
                new Point(jaw_OffsetX + individual_tooth_OffsetX,
                            offsetY),
                new Point(jaw_OffsetX + individual_tooth_OffsetX,
                            offsetY + organism.GenotypeHeights[Genes.TEETH_GENE]));
            
            // Tooth3 (Offset from tooth2)
            jaw_OffsetX += individual_tooth_OffsetX;
            drawingContext.DrawLine(new Pen(Brushes.Black, 1.0),
                new Point(jaw_OffsetX + individual_tooth_OffsetX,
                            offsetY),
                new Point(jaw_OffsetX + individual_tooth_OffsetX,
                            offsetY + organism.GenotypeHeights[Genes.TEETH_GENE]));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            IEvolvableSkull organism = this.DataContext as IEvolvableSkull;
            if (organism != null)
            {
                organism.Parent.NewPopulationFromDominant(organism);
            }
        }
    }
}
