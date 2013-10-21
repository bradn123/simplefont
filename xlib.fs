clear-libs
\c #include <stdlib.h>
c-function rand rand -- n

s" X11 -L/opt/local/lib" add-lib
\c #include <X11/Xlib.h>

\c #define XEventSize() sizeof(XEvent)
\c #define XEventType(e) (((XEvent*)(e))->type)

\c #define ExposureMaskValue() ExposureMask
\c #define KeyPressMaskValue() KeyPressMask
\c #define ButtonPressMaskValue() ButtonPressMask
\c #define ExposeValue() Expose

\c #define XColorSize() sizeof(XColor)
\c #define XColorPixel(c) (((XColor*)(c))->pixel)
\c #define XColorSetRed(v, c) (((XColor*)(c))->red = v)
\c #define XColorSetGreen(v, c) (((XColor*)(c))->green = v)
\c #define XColorSetBlue(v, c) (((XColor*)(c))->blue = v)
\c #define XColorSetFlags(v, c) (((XColor*)(c))->flags = v)

c-function XOpenDisplay XOpenDisplay a -- a
c-function XCreateSimpleWindow XCreateSimpleWindow a n n n n n n n n -- n
c-function RootWindow RootWindow a n -- n
c-function DefaultScreen DefaultScreen a -- n
c-function BlackPixel BlackPixel a n -- n
c-function WhitePixel WhitePixel a n -- n
c-function XMapWindow XMapWindow a n -- void
c-function XNextEvent XNextEvent a a -- n
c-function XSelectInput XSelectInput a n n -- void
c-function XCreateGC XCreateGC a n n a -- a
c-function XFillRectangle XFillRectangle a n a n n n n -- void
c-function XDrawPoint XDrawPoint a n a n n -- void
c-function XSetForeground XSetForeground a a n -- void
c-function DefaultColormap DefaultColormap a n -- n

c-function XEventSize XEventSize -- n
c-function XEventType XEventType a -- n

c-function XColorSize XColorSize -- n
c-function XColorPixel XColorPixel a -- n
c-function XColorSetRed XColorSetRed n a -- void
c-function XColorSetGreen XColorSetGreen n a -- void
c-function XColorSetBlue XColorSetBlue n a -- void
c-function XColorSetFlags XColorSetFlags n a -- void

c-function XAllocColor XAllocColor a n a -- void

c-function ExposureMask ExposureMaskValue -- n
c-function KeyPressMask KeyPressMaskValue -- n
c-function ButtonPressMask ButtonPressMaskValue -- n
c-function Expose ExposeValue -- n


0 constant NULL

variable display   NULL XOpenDisplay display !
variable screen   display @ DefaultScreen screen !
display @ screen @ BlackPixel constant black
display @ screen @ WhitePixel constant white
variable window
  display @ dup 0 RootWindow 1 1 1000 1000 0 white white
  XCreateSimpleWindow window !
display @ window @ XMapWindow
create event XEventSize allot
variable gc  display @ window @ 0 NULL XCreateGC gc !

variable colormap  display @ screen @ DefaultColorMap colormap !
create xcolor XColorSize allot

: random ( n -- n ) rand swap mod ;
: rgb ( r g b -- pixel )
  xcolor XColorSetBlue xcolor XColorSetGreen  xcolor XColorSetRed
  0 xcolor XColorSetFlags
  display @ colormap @ xcolor XAllocColor 
  xcolor XColorPixel ;
: gray ( n -- pixel ) dup dup rgb ;

65535 0 0 rgb constant red
0 65535 0 rgb constant green
65535 65535 0 rgb constant yellow

: plot ( x y -- ) display @ window @ gc @ 4 roll 4 roll XDrawPoint ;
: pen ( c -- ) display @ swap gc @ swap XSetForeground ;
: rect ( x y w h -- )
  display @ window @ gc @ 6 roll 6 roll 6 roll 6 roll XFillRectangle ;

: plot 2* swap 2* swap 2 2 rect ;
: rect >r >r >r 2* r> 2* r> 2* r> 2* rect ;

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
  

display @ window @ ExposureMask KeyPressMask or ButtonPressMask or XSelectInput
: run 
  begin
    display @ event XNextEvent
    event XEventType Expose if
(
      white pen
      0 0 500 500 rect
      green pen
      200 100 500 500 rect
      yellow pen
      0 200 500 500 rect
      red pen
      100 0 do
        500 random 500 random 50 50 rect
      loop
)
      black pen
      50000 0 0 300 20000 400 quartic
      50000 0 50000 0 20000 400 quartic
      0 0 50000 0 0 500 quartic
\      200 50 100 311 line
    then
  again
;
run
