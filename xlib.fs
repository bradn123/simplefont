( Bindings for Xlib )

clear-libs
0 constant NULL

\c #include <stdlib.h>
c-function rand rand -- n
c-function malloc malloc n -- a

s" X11 -L/opt/local/lib" add-lib  \ For OSX
\c #include <X11/Xlib.h>

\c #define XEventSize() sizeof(XEvent)
c-function XEventSize XEventSize -- n
\c #define XEventType(e) (((XEvent*)(e))->type)
c-function XEventType XEventType a -- n
\c #define XEventConfigureWidth(e) (((XEvent*)(e))->xconfigure.width)
c-function XEventConfigureWidth XEventConfigureWidth a -- n
\c #define XEventConfigureHeight(e) (((XEvent*)(e))->xconfigure.height)
c-function XEventConfigureHeight XEventConfigureHeight a -- n

\c #define ButtonPressMaskValue() ButtonPressMask
c-function ButtonPressMask ButtonPressMaskValue -- n
\c #define ConfigureNotifyValue() ConfigureNotify
c-function ConfigureNotify ConfigureNotifyValue -- n
\c #define ExposeValue() Expose
c-function Expose ExposeValue -- n
\c #define ExposureMaskValue() ExposureMask
c-function ExposureMask ExposureMaskValue -- n
\c #define KeyPressMaskValue() KeyPressMask
c-function KeyPressMask KeyPressMaskValue -- n
\c #define StructureNotifyMaskValue() StructureNotifyMask
c-function StructureNotifyMask StructureNotifyMaskValue -- n
\c #define ZPixmapValue() ZPixmap
c-function ZPixmap ZPixmapValue -- n

\c #define XColorSize() sizeof(XColor)
c-function XColorSize XColorSize -- n
\c #define XColorPixel(c) (((XColor*)(c))->pixel)
c-function XColorPixel XColorPixel a -- n
\c #define XColorSetRed(v, c) (((XColor*)(c))->red = v)
c-function XColorSetRed XColorSetRed n a -- void
\c #define XColorSetGreen(v, c) (((XColor*)(c))->green = v)
c-function XColorSetGreen XColorSetGreen n a -- void
\c #define XColorSetBlue(v, c) (((XColor*)(c))->blue = v)
c-function XColorSetBlue XColorSetBlue n a -- void
\c #define XColorSetFlags(v, c) (((XColor*)(c))->flags = v)
c-function XColorSetFlags XColorSetFlags n a -- void

c-function BlackPixel BlackPixel a n -- n
c-function DefaultColormap DefaultColormap a n -- n
c-function DefaultScreen DefaultScreen a -- n
c-function RootWindow RootWindow a n -- n
c-function WhitePixel WhitePixel a n -- n
c-function XAllocColor XAllocColor a n a -- void
c-function XCreateGC XCreateGC a n n a -- a
c-function XCreateImage XCreateImage a a n n n a n n n n -- a
c-function XCreateSimpleWindow XCreateSimpleWindow a n n n n n n n n -- n
c-function XDefaultDepth XDefaultDepth a n -- n
c-function XDefaultVisual XDefaultVisual a n -- a
c-function XDestroyImage XDestroyImage a -- void
c-function XDrawPoint XDrawPoint a n a n n -- void
c-function XFillRectangle XFillRectangle a n a n n n n -- void
c-function XFlush XFlush a -- void
c-function XMapWindow XMapWindow a n -- void
c-function XNextEvent XNextEvent a a -- n
c-function XOpenDisplay XOpenDisplay a -- a
c-function XPutImage XPutImage a n a a n n n n n n -- void
c-function XSelectInput XSelectInput a n n -- void
c-function XSetForeground XSetForeground a a n -- void
