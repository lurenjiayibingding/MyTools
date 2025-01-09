package toolstorage

import (
	"fmt"
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
