using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System.Drawing.Imaging;

public class GifHandler{

	public List<Texture2D> GifToTextureList(Image image)
    {
        List<Texture2D> textureArray = null;

        if(image != null)
        {
            textureArray = new List<Texture2D>();
            //Debug.Log("There is total " + image.FrameDimensionsList.Length + " frame in the list");
            FrameDimension frameDimension = new FrameDimension(image.FrameDimensionsList[0]);
            int frameCount = image.GetFrameCount(frameDimension);

            for(int i = 0; i < frameCount; i++)
            {
                image.SelectActiveFrame(frameDimension, i);
                var frameBitmap = new Bitmap(image.Width, image.Height);

                System.Drawing.Graphics.FromImage(frameBitmap).DrawImage(image, Point.Empty);

                var frameTexture2D = new Texture2D(frameBitmap.Width, frameBitmap.Height);
                for(int x = 0; x < frameBitmap.Width; x++)
                {
                    for(int y = 0; y < frameBitmap.Height; y++)
                    {
                        System.Drawing.Color sourceColor = frameBitmap.GetPixel(x, y);
                        frameTexture2D.SetPixel(x, frameBitmap.Height - 1 - y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A));

                    }
                }
                frameTexture2D.Apply();
                textureArray.Add(frameTexture2D);
            }
        }

        return textureArray;
    }

    public List<Sprite> GifToSpriteList(Image image)
    {
        List<Sprite> spriteArray = null;

        if (image != null)
        {
            //textureArray = new List<Texture2D>();
            spriteArray = new List<Sprite>();

            //Debug.Log("There is total " + image.FrameDimensionsList.Length + " frame in the list");
            FrameDimension frameDimension = new FrameDimension(image.FrameDimensionsList[0]);
            int frameCount = image.GetFrameCount(frameDimension);

            for (int i = 0; i < frameCount; i++)
            {
                image.SelectActiveFrame(frameDimension, i);
                var frameBitmap = new Bitmap(image.Width, image.Height);

                System.Drawing.Graphics.FromImage(frameBitmap).DrawImage(image, Point.Empty);

                var frameTexture2D = new Texture2D(frameBitmap.Width, frameBitmap.Height);
                for (int x = 0; x < frameBitmap.Width; x++)
                {
                    for (int y = 0; y < frameBitmap.Height; y++)
                    {
                        System.Drawing.Color sourceColor = frameBitmap.GetPixel(x, y);
                        frameTexture2D.SetPixel(x, frameBitmap.Height - 1 - y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A));

                    }
                }
                frameTexture2D.Apply();
                Sprite nSprite = Sprite.Create(frameTexture2D, new Rect(0.0f, 0.0f, frameTexture2D.width, frameTexture2D.height), new Vector2(0.0f, 0.0f));
                spriteArray.Add(nSprite);
            }
        }

        return spriteArray;
    }
}
