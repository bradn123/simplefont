s" xlib.fs" included

( ------------------------------------------------------------ )
variable display  NULL XOpenDisplay display !
variable screen  display @ DefaultScreen screen !
variable colormap  display @ screen @ DefaultColorMap colormap !
variable visual  display @ screen @ XDefaultVisual visual !
variable screen-depth  display @ screen @
    XDefaultDepth screen-depth !
display @ screen @ BlackPixel constant black
display @ screen @ WhitePixel constant white

create xevent XEventSize allot

variable window-handle
variable gc

: window ( w h -- )
  >r >r
  display @ dup 0 RootWindow 1 1 r> r> 0 black white
  XCreateSimpleWindow window-handle !
  display @ window-handle @ XMapWindow
  display @ window-handle @ 0 NULL XCreateGC gc !
  display @ window-handle @
  ExposureMask
  ButtonPressMask or
  ButtonReleaseMask or
  KeyPressMask or
  KeyReleaseMask or
  PointerMotionMask or
  StructureNotifyMask or
  XSelectInput
;

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

1024 constant expose-event
1025 constant resize-event

variable mouse-position-x
variable mouse-position-y

: mouse-x ( -- n ) mouse-position-x @ ;
: mouse-y ( -- n ) mouse-position-y @ ;

: update-mouse ( -- )
  xevent XEventX mouse-position-x !
  xevent XEventY mouse-position-y !
;

variable last-event
variable last-keysym-value
80 constant key-buffer-size
variable key-buffer-length
create key-buffer key-buffer-size allot

: event ( -- n ) last-event @ ;
: last-keysym ( -- n ) last-keysym-value @ ;
: last-key ( -- a n ) key-buffer key-buffer-length @ ;

: update-keys
  xevent XEventKeyCode last-event !
  xevent XEventKeyEvent key-buffer key-buffer-size
  last-keysym-value NULL XLookupString key-buffer-length !
;

: wait ( -- )
  display @ xevent XNextEvent
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
    0 last-event !
    exit
  then
  0 last-event !
;

( ------------------------------------------------------------ )
\ window ( w h -- )
\ pixel ( x y -- a )
\ width ( -- n )
\ height ( -- n )
\ mouse-x ( -- n )
\ mouse-y ( -- n )
\ event ( -- n )
\ last-key ( -- n )
\ flip ( -- )
\ wait ( -- )
\
\ expose-event ( -- n ) 256 
\ 32 thru 126 keydown
\ -32 thru -126 keyup
\ 1 thru 3 mousedown
\ -1 thru -3 mouseup
\ 0 mousemove

( ------------------------------------------------------------ )


( ------------------------------------------------------------ )
\ UNUSED

create xcolor XColorSize allot

: rgb ( r g b -- pixel )
  xcolor XColorSetBlue
  xcolor XColorSetGreen
  xcolor XColorSetRed
  0 xcolor XColorSetFlags
  display @ colormap @ xcolor XAllocColor 
  xcolor XColorPixel ;
: gray ( n -- pixel ) dup dup rgb ;

: plot ( x y -- )
  display @ window-handle @ gc @ 4 roll 4 roll XDrawPoint ;
: pen ( c -- ) display @ swap gc @ swap XSetForeground ;
: rect ( x y w h -- )
  display @ window-handle @ gc @
  6 roll 6 roll 6 roll 6 roll XFillRectangle ;

( ------------------------------------------------------------ )
256 constant blend
blend 1- constant blend'

: lband ( x y -- )
  over blend mod 65535 blend' */ gray pen
  swap blend / swap plot
;

: rband ( x y -- )
  over blend mod blend' swap - 65535 blend' */ gray pen
  swap blend / 1- swap plot
;

: band ( x y w -- )
  black pen >r 2dup swap blend / swap r> dup >r blend / 1 rect r>
  >r 2dup swap r> + swap rband
  lband
;
  

( ------------------------------------------------------------ )
: point   create 2 cells allot ;
: p! ( x y a -- ) swap over cell+ ! ! ;
: p@ ( a -- x y ) dup @ swap cell+ @ ;
: x@ ( a -- x ) @ ;
: y@ ( a -- y ) cell+ @ ;

: square ( n -- n ) dup * ;
: distance2 ( x1 y1 x2 y2 ) rot - square -rot - square + ;
: middle ( x1 y1 x2 y2 -- mx my ) rot + 2/ >r + 2/ r> ;
: middle' ( x1 y1 x2 y2 -- mx my ) rot + 1+ 2/ >r + 1+ 2/ r> ;
point q1  point q2  point q3  point q12  point q23  point q123
point q12' point q23' point q123'
: sp space ;
: quartic ( p1 p2 p3 -- )
  q3 p! q2 p! q1 p!
  q1 y@ q3 y@ - 0= if
    q1 p@ 2000 band
  else
    q1 p@ q2 p@ middle q12 p!
    q2 p@ q3 p@ middle q23 p!
    q1 p@ q2 p@ middle' q12' p!
    q2 p@ q3 p@ middle' q23' p!
    q12 p@ q23' p@ middle q123 p!
    q12' p@ q23 p@ middle' q123' p!
    q1 p@ q12 p@ q123 p@
    q123' p@ q23' p@ q3 p@
    recurse recurse
  then 
;

: line ( p1 p2 -- )
  q2 p! q1 p!
  q1 p@ q2 p@ distance2 2 < if
    q1 p@ 10 1 rect
  else
    q1 p@ q2 p@ middle q12 p!
    q1 p@ q2 p@ middle' q12' p!
    q1 p@ q12 p@
    q12 p@ q2 p@
    recurse recurse
  then
;
