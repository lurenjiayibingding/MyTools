package toolstorage

import (
	"fmt"
	"image"
	"image/color"
	"image/jpeg"
	"image/png"
	"os"
)

func SavePNGToJPG(inputPath string, outputPath string) error {
	pngFile, err := os.Open(inputPath)
	if err != nil {
		fmt.Print(err)
		return err
	}
	defer pngFile.Close()

	img, err := png.Decode(pngFile)
	if err != nil {
		fmt.Print(err)
		return err
	}

	jpgFile, err := os.Create(outputPath)
	if err != nil {
		fmt.Print(err)
		return err
	}
	defer jpgFile.Close()

	opt := jpeg.Options{
		Quality: 100,
	}

	err = jpeg.Encode(jpgFile, img, &opt)
	if err != nil {
		fmt.Print(err)
		return err
	}

	fmt.Print("图片" + inputPath + "转为" + outputPath + "成功")
	return nil
}

// 创建一个指定宽度、高度和rgb颜色值的jgp图片。
// width:图片宽度，height:图片高度，r:红色值，g:绿色值，b:蓝色值，savePath:保存路径
func CreateJpgPricture(width, height int, r, g, b uint8, savePath string) error {
	img := image.NewRGBA(image.Rect(0, 0, width, height))
	//A:255表示完全不透明
	//go中实例化一个结构体时，要么带着全部的参数名，要么全部不带，不能只带一部分
	colorValue := color.RGBA{r, g, b, 255}
	// colorValue := color.RGBA{R: r, G: g, B: b, A: 255}

	//将颜色填充到整个图片中
	for y := 0; y < height; y++ {
		for x := 0; x < width; x++ {
			img.Set(x, y, colorValue)
		}
	}

	file, err := os.Create(savePath)
	if err != nil {
		fmt.Print(err)
		return err
	}
	defer file.Close()

	options := jpeg.Options{Quality: 100}

	err1 := jpeg.Encode(file, img, &options)
	if err1 != nil {
		fmt.Print(err1)
		return err1
	}
	print("图片" + savePath + "创建成功")
	return nil
}
