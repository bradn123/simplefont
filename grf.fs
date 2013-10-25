s" xlib.fs" included

( ------------------------------------------------------------ )
( Public interface )

\ Startup:
\   window ( w h -- )
\ Drawing region:
\   pixel ( x y -- a ) (format [b g r x])
\   width ( -- n )
\   height ( -- n )
\   flip ( -- )
\ Getting events:
\   wait ( -- )
\   poll ( -- )
\ Event info:
\   mouse-x ( -- n )
\   mouse-y ( -- n )
\   last-key ( -- n )
\   last-keysym ( -- n )
\   event ( -- n )
\ Event constants:
\   motion-event
\   expose-event
\   resize-event
\   timeout-event
\   unknown-event
\   32 thru 126 keydown
\   -32 thru -126 keyup
\   1 thru 3 mousedown
\   -1 thru -3 mouseup


( ------------------------------------------------------------ )
( Public event constants )

0 constant motion-event
1024 constant expose-event
1025 constant resize-event
1026 constant timeout-event
1027 constant unknown-event


( ------------------------------------------------------------ )
( Globals for display and visuals )

variable display  NULL XOpenDisplay display !
variable screen  display @ DefaultScreen screen !
variable colormap  display @ screen @ DefaultColorMap colormap !
variable visual  display @ screen @ XDefaultVisual visual !
variable screen-depth  display @ screen @
    XDefaultDepth screen-depth !
display @ screen @ BlackPixel constant black
display @ screen @ WhitePixel constant white


( ------------------------------------------------------------ )
( Window creation )

ExposureMask
ButtonPressMask or
ButtonReleaseMask or
KeyPressMask or
KeyReleaseMask or
PointerMotionMask or
StructureNotifyMask or constant event-mask

variable window-handle
variable gc

: window ( w h -- )
  >r >r
  display @ dup 0 RootWindow 1 1 r> r> 0 black white
  XCreateSimpleWindow window-handle !
  display @ window-handle @ XMapWindow
  display @ window-handle @ 0 NULL XCreateGC gc !
  display @ window-handle @ event-mask XSelectInput
;


( ------------------------------------------------------------ )
( Image handling )

variable image
variable image-width  variable image-height
variable image-data

: width ( -- n ) image-width @ ;
: height ( -- n ) image-height @ ;
: pixel ( x y -- a ) image-width @ * + 4 * image-data @ + ;

: image-resize ( w h -- )
  image-height !  image-width !
  image @ 0<> if image @ XDestroyImage then
  image-width @ image-height @ * 4 * malloc image-data !
  display @ visual @ screen-depth @ ZPixmap 0
  image-data @ image-width @ image-height @
  32 image-width @ 4 * XCreateImage image !
;
1 1 image-resize  ( Make sure something's allocated. )

: flip ( -- )
  display @ window-handle @ gc @ image @
  0 0 0 0 image-width @ image-height @ XPutImage
  \ display @ XFlush
;


( ------------------------------------------------------------ )
( Event handling )

( Keyboard )
80 constant last-key-buffer-size
create last-key-buffer last-key-buffer-size allot
variable last-key-buffer-length
variable last-keysym-value
( Mouse )
variable mouse-position-x
variable mouse-position-y
( Event )
variable last-event
create xevent XEventSize allot

: mouse-x ( -- n ) mouse-position-x @ ;
: mouse-y ( -- n ) mouse-position-y @ ;
: event ( -- n ) last-event @ ;
: last-keysym ( -- n ) last-keysym-value @ ;
: last-keys ( -- a n ) last-key-buffer last-key-buffer-length @ ;
: last-key ( -- n ) last-keys 0> if c@ else drop 0 then ;

: update-mouse ( -- )
  xevent XEventX mouse-position-x !
  xevent XEventY mouse-position-y !
;

: update-keys
  xevent XEventKeyCode last-event !
  xevent XEventKeyEvent last-key-buffer last-key-buffer-size
  last-keysym-value NULL XLookupString last-key-buffer-length !
;

: update-last-event ( -- )
  xevent XEventType Expose = if
     expose-event last-event !
     exit
  then
  xevent XEventType ConfigureNotify = if
    xevent XEventConfigureWidth
    xevent XEventConfigureHeight image-resize
    resize-event last-event !
    exit
  then
  xevent XEventType KeyPress = if
    update-mouse
    update-keys
    exit
  then
  xevent XEventType KeyRelease = if
    update-mouse
    update-keys
    last-event @ negate last-event !
    exit
  then
  xevent XEventType ButtonPress = if
    update-mouse
    xevent XEventButton last-event ! ( uses carnal knowlege )
    exit
  then
  xevent XEventType ButtonRelease = if
    update-mouse
    xevent XEventButton negate last-event ! ( uses carnal knowlege )
    exit
  then
  xevent XEventType MotionNotify = if
    update-mouse
    motion-event last-event !
    exit
  then
  unknown-event last-event !
;

: wait ( -- )
  display @ xevent XNextEvent
  update-last-event
;

: poll ( -- )
  display @ event-mask xevent XCheckMaskEvent
  0= if
    timeout-event last-event !
  else
    update-last-event
  then
;
