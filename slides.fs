s" grf.fs" included
s" font.fs" included
s" gimple1.fs" included
( ------------------------------------------------------------ )


( ------------------------------------------------------------ )
( Formatting utilities )

: centered ( n -- ) width swap font-width @ * - 2/ font-x ! ;
: center-type ( a n -- ) dup centered font-type font-cr ;
: left-type ( a n -- ) font-type font-cr ;
: font-size! ( n -- ) width over / font-width !
                      height 2* swap / font-height ! ;
: font-pick ( i b cols -- ) font-size!
                            width 10 */ font-weight!
                            50 * font-slant ! ;
: super-big   0 5 20 font-pick ;
: big   0 3 30 font-pick ;
: normal  0 1 44 font-pick ;
: bullet  0 5 44 font-pick font-cr s"   ~ " font-type normal ;
: indent1  s"  " font-type ;
: indent4  normal s"     " font-type ;
: .f"   postpone s" postpone left-type ; immediate
: *f"   postpone bullet postpone .f" ; immediate
: +f"   postpone indent4 postpone .f" ; immediate
: .title"   postpone big postpone home postpone indent1
            postpone .f" postpone font-cr ; immediate

( ------------------------------------------------------------ )

: title
  super-big home
  font-cr font-cr
  s" Simple Scalable" center-type
  s" Fonts" center-type
  font-cr normal
  s" Brad Nelson" center-type
  s" November 16, 2013" center-type
;

( ------------------------------------------------------------ )

: motivation
  .title" Motivation"
  *f" Fonts are a sizeable part of the"
  +f" footprint of even the simplest"
  +f" system"
  *f" Letters can be things of beauty"
  *f" Small is cool"
;

( ------------------------------------------------------------ )

: bezier-curves
  .title" Bezier Curves"
  *f" Described by 3 or more control points"
  *f" End points are one the curve"
  *f" End segments are tangent to the curve"
;

( ------------------------------------------------------------ )

: de-castelijau-subdivision
  .title" De Castelijau Subdivision"
  *f" Divide a Bezier curve in two"
  *f" Just + and 2/ for the middle"
;

( ------------------------------------------------------------ )

: quartic-curves-in-forth
  .title" Quartic Curves in Forth"
  *f" Use fixed-point with 8-bits of fraction"
  *f" Variable to assemble points, then fill"
  +f" the stack before tree recursion."
;

( ------------------------------------------------------------ )

: quartic-drawing
  .title" Quartic Drawing"
  +f" ( Quartic bezier curve plotting )"
  +f" point q1  point q2  point q3"
  +f" point q12  point q23  point q123"
  +f" : quartic' ( p1 p2 p3 -- )"
  +f"   q3 p! q2 p! q1 p!"
  +f"   q1 p@ q3 p@ distance2 granual2 < if"
  +f"     q1 p@ q3 p@ middle penplot"
  +f"   else"
  +f"     q1 p@ q2 p@ middle q12 p!"
  +f"     q2 p@ q3 p@ middle q23 p!"
  +f"     q12 p@ q23 p@ middle q123 p!"
  +f"     q1 p@ q12 p@ q123 p@"
  +f"     q123 p@ q23 p@ q3 p@"
  +f"     recurse recurse"
  +f"   then"
  +f" ;"
;

( ------------------------------------------------------------ )

: font-layout
  .title" Font Layout"
  *f" 2 x 4 unit layout grid"
  *f" padded for descenders + control"
;

( ------------------------------------------------------------ )

: anti-aliased-pen
  .title" Anti-aliased Pen"
  *f" Draw an anti-aliased filled circle"
  *f" Blend with other circles using min/max"
  *f" Calculate distance in a window"
  +f" -> sqrt :-("
;

( ------------------------------------------------------------ )

: font-format
  .title" Font Format"
  +f" ( a )  425624 /, 420244 /, /;,"
  +f" ( b )  222426 /, 226325 /, /;,"
  +f" ( c )  420245 /, /;,"
  +f" ( d )  424446 /, 420345 /, /;,"
  +f" ( e )  420135 /, 355422 /, /;,"
  +f" ( f )  322646 /, 244444 /, /;,"
  +f" ( g )  215145 /, 420345 /, /;,"
  +f" ( h )  222426 /, 234642 /, /;,"
  +f" ( i )  422234 /, 353636 /, /;,"
;

( ------------------------------------------------------------ )

: memory-format
  .title" Memory Format"
  *f" One byte per point 00 - 99
  *f" 3 points per quartic"
  *f" End marked by tagging high bit of"
  +f" first byte in the last triplet"
;

( ------------------------------------------------------------ )
( Slide deck )

create slides
' title ,
' motivation ,
' bezier-curves ,
' de-castelijau-subdivision ,
' quartic-curves-in-forth ,
' quartic-drawing ,
' font-layout ,
' anti-aliased-pen ,
' font-format ,
' memory-format ,
 here slides - cell / constant slide-count


( ------------------------------------------------------------ )
( Slide drawing )

variable slide
: draw   slides slide @ cells + @ clear execute flip ;
: slide-clip   slide @ 0 max slide-count 1- min slide ! ;
: slide-step ( n -- ) slide +! slide-clip draw ;
: backward -1 slide-step ;
: forward 1 slide-step ;

( ------------------------------------------------------------ )
( Main loop )
131 constant left-key
132 constant right-key
61 constant escape-key
57 constant space-key

: slideshow
  gimple1 font !
  ['] font-type is type
  ['] font-emit is emit
  1024 768 window
  begin
    wait
    event case
      expose-event of expose-count 0= if draw then endof
      press-event of
        last-keycode case
          left-key of backward endof
          right-key of forward endof
          space-key of forward endof
          escape-key of bye endof
        endcase
      endof
    endcase
  again
;
slideshow
