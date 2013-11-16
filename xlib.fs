( Bindings for Xlib )

clear-libs
0 constant NULL

\c #include <stdlib.h>
c-function rand rand -- n
c-function malloc malloc n -- a

s" X11 -L/opt/local/lib" add-lib  ( For OSX )
\c #include <X11/Xlib.h>
\c #define XUTIL_DEFINE_FUNCTIONS
\c #include <X11/Xutil.h>

( Accessors for members of XEvent )
\c #define XEventSize() sizeof(XEvent)
c-function XEventSize XEventSize -- n
\c #define XEventType(e) (((XEvent*)(e))->type)
c-function XEventType XEventType a -- n
\c #define XEventConfigureWidth(e) (((XEvent*)(e))->xconfigure.width)
c-function XEventConfigureWidth XEventConfigureWidth a -- n
\c #define XEventConfigureHeight(e) (((XEvent*)(e))->xconfigure.height)
c-function XEventConfigureHeight XEventConfigureHeight a -- n
\c #define XEventKeyEvent(e) (&((XEvent*)(e))->xkey)
c-function XEventKeyEvent XEventKeyEvent a -- a
\c #define XEventKeycode(e) (((XEvent*)(e))->xkey.keycode)
c-function XEventKeycode XEventKeycode a -- n
\c #define XEventButton(e) (((XEvent*)(e))->xbutton.button)
c-function XEventButton XEventButton a -- n
\c #define XEventX(e) (((XEvent*)(e))->xmotion.x)
c-function XEventX XEventX a -- n
\c #define XEventY(e) (((XEvent*)(e))->xmotion.y)
c-function XEventY XEventY a -- n
\c #define XEventExposeCount(e) (((XEvent*)(e))->xexpose.count)
c-function XEventExposeCount XEventExposeCount a -- n

( Various event masks and event ids )
\c #define ButtonPressValue() ButtonPress
c-function ButtonPress ButtonPressValue -- n
\c #define ButtonPressMaskValue() ButtonPressMask
c-function ButtonPressMask ButtonPressMaskValue -- n
\c #define ButtonReleaseValue() ButtonRelease
c-function ButtonRelease ButtonReleaseValue -- n
\c #define ButtonReleaseMaskValue() ButtonReleaseMask
c-function ButtonReleaseMask ButtonReleaseMaskValue -- n
\c #define ConfigureNotifyValue() ConfigureNotify
c-function ConfigureNotify ConfigureNotifyValue -- n
\c #define ExposeValue() Expose
c-function Expose ExposeValue -- n
\c #define ExposureMaskValue() ExposureMask
c-function ExposureMask ExposureMaskValue -- n
\c #define KeyPressValue() KeyPress
c-function KeyPress KeyPressValue -- n
\c #define KeyPressMaskValue() KeyPressMask
c-function KeyPressMask KeyPressMaskValue -- n
\c #define KeyReleaseValue() KeyRelease
c-function KeyRelease KeyReleaseValue -- n
\c #define KeyReleaseMaskValue() KeyReleaseMask
c-function KeyReleaseMask KeyReleaseMaskValue -- n
\c #define MotionNotifyValue() MotionNotify
c-function MotionNotify MotionNotifyValue -- n
\c #define PointerMotionMaskValue() PointerMotionMask
c-function PointerMotionMask PointerMotionMaskValue -- n
\c #define StructureNotifyMaskValue() StructureNotifyMask
c-function StructureNotifyMask StructureNotifyMaskValue -- n
\c #define ZPixmapValue() ZPixmap
c-function ZPixmap ZPixmapValue -- n

( Xlib functions / macros )
c-function BlackPixel BlackPixel a n -- n
c-function DefaultColormap DefaultColormap a n -- n
c-function DefaultScreen DefaultScreen a -- n
c-function RootWindow RootWindow a n -- n
c-function WhitePixel WhitePixel a n -- n
c-function XCheckMaskEvent XCheckMaskEvent a n a -- n
c-function XCreateGC XCreateGC a n n a -- a
c-function XCreateImage XCreateImage a a n n n a n n n n -- a
c-function XCreateSimpleWindow XCreateSimpleWindow a n n n n n n n n -- n
c-function XDefaultDepth XDefaultDepth a n -- n
c-function XDefaultVisual XDefaultVisual a n -- a
c-function XDestroyImage XDestroyImage a -- void
c-function XFlush XFlush a -- void
c-function XLookupString XLookupString a a n a a -- n
c-function XMapWindow XMapWindow a n -- void
c-function XNextEvent XNextEvent a a -- void
c-function XOpenDisplay XOpenDisplay a -- a
c-function XPutImage XPutImage a n a a n n n n n n -- void
c-function XSelectInput XSelectInput a n n -- void
