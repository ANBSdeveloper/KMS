export class ImageResizer {
  compressImage(src, newX, newY) {
    return new Promise((res, rej) => {
      const img = new Image();
      img.src = src;
      img.onload = () => {
        const elem = document.createElement("canvas");
        elem.width = newX;
        elem.height = newY;
        const ctx = elem.getContext("2d");
        ctx.drawImage(img, 0, 0, newX, newY);
        const data = ctx.canvas.toDataURL();
        res(data);
      };
      img.onerror = (error) => rej(error);
    });
  }
}

export const imageResize = new ImageResizer();
