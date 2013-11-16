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
: font-pick ( b cols -- ) font-size!
                          width 20 */ font-weight!
                          0 font-slant ! ;
: super-big   10 20 font-pick ;
: big   6 30 font-pick ;
: normal  2 44 font-pick ;
: tiny  1 100 font-pick ;
: bullet  10 44 font-pick font-cr s"   ~ " font-type normal ;
: bspace font-width @ negate font-x +! ;
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

: ground-rules
  .title" Ground Rules"
  *f" Use gforth"
  *f" Wrap bitmapped graphics + events"
  +f" in Xlib to emulate a simple raster"
  +f" display."
  *f" Integer math only"
  *f" Use smallness as an artistic constraint"
;

( ------------------------------------------------------------ )

: simple-graphics
  .title" Simple Graphics"
  +f" Startup:"
  +f"  window ( w h -- )"
  +f" Drawing region:"
  +f"  pixel ( x y -- a ) (format [b g r x])"
  +f"  width ( -- n )"
  +f"  height ( -- n )"
  +f"  flip ( -- )"
;

( ------------------------------------------------------------ )

: simple-events
  .title" Simple Events"
  +f" Getting events:"
  +f"   wait ( -- )"
  +f"   poll ( -- )"
  +f" Event info:"
  +f"  mouse-x ( -- n )"
  +f"  mouse-y ( -- n )"
  +f"  last-key ( -- n )"
  +f"  last-keysym ( -- n )"
  +f"  last-keycode ( -- n )"
  +f"  event ( -- n )"
;

( ------------------------------------------------------------ )

: bezier   ."  Be" bspace 30 font-slant ! ." '"
           0 font-slant ! ." zier" ;
: bezier-curves
  big home bezier ."  Curves" font-cr
  *f" Described by 3 or more control points"
  *f" End points are one the curve"
  *f" End segments are tangent to the curve"
  width 3 10 */ height 6 10 */
  width 7 10 */ height 7 10 */ line
  width 7 10 */ height 7 10 */
  width 3 10 */ height 8 10 */ line
  20 10 font-pick
  width 3 10 */ height 6 10 */
  width 7 10 */ height 7 10 */
  width 3 10 */ height 8 10 */ quartic
;

( ------------------------------------------------------------ )

: de-castelijau-subdivision
  .title" De Castelijau Subdivision"
  bullet normal ." Divide a " bezier ."  curve in two" font-cr
  *f" Just + and 2/ for the middle"
  width 3 10 */ height 6 10 */
  width 7 10 */ height 7 10 */ line
  width 7 10 */ height 7 10 */
  width 3 10 */ height 8 10 */ line
  5 10 font-pick
  width 3 10 */ height 6 10 */
  width 5 10 */ height 65 100 */ line
  width 5 10 */ height 75 100 */
  width 3 10 */ height 8 10 */ line
  width 5 10 */ height 65 100 */
  width 5 10 */ height 75 100 */ line
  20 10 font-pick
  width 3 10 */ height 6 10 */
  width 7 10 */ height 7 10 */
  width 3 10 */ height 8 10 */ quartic
;

( ------------------------------------------------------------ )

: quartic-curves-in-forth
  .title" Quartic Curves in Forth"
  *f" Use fixed-point with 8-bits of fraction"
  *f" Variables to assemble points,"
  +f" then fill the data stack before"
  +f" recursion."
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

: anti-aliased-pen
  .title" Anti-aliased Pen"
  *f" Draw an anti-aliased filled circle"
  *f" Blend with other circles using min/max"
  *f" Calculate distance in a window"
  +f" -> sqrt :-("
;

( ------------------------------------------------------------ )

: grid-x 10 + width 25 */ ;
: grid-y 14 + height 30 */ ;
: grid-xy swap grid-x swap 10 swap - grid-y ;
: font-layout
  .title" Font Layout"
  *f" 7 x 10 unit layout grid"
  *f" 2 x 4 core padded for"
  +f" descenders + control"
  7 0 do i 0 grid-xy i 10 grid-xy line loop
  11 0 do 0 i grid-xy 6 i grid-xy line loop
  big
  4 2 grid-xy 5 6 grid-xy line
  5 6 grid-xy 2 4 grid-xy line
  4 2 grid-xy 0 2 grid-xy line
  0 2 grid-xy 4 4 grid-xy line
  20 10 font-pick
  4 2 grid-xy 5 6 grid-xy 2 4 grid-xy quartic
  4 2 grid-xy 0 2 grid-xy 4 4 grid-xy quartic
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

: gimple1-slide
  .title" Gimple1"
  5 44 font-pick
  ."     " 64 32 do i emit loop font-cr
  ."     " 96 64 do i emit loop font-cr
  ."     " 128 96 do i emit loop font-cr
  *f" Smallest nice font I could fit"
  *f" 95 visible ASCII characters"
  *f" ~9k as a TrueType font"
  *f" ~512 bytes in the current encoding"
  *f" 2 strokes for numbers and lowercase"
  +f" letters"
  *f" no more then 3 strokes in any glyph"
;

( ------------------------------------------------------------ )

: size-can-vary
  .title" Size Can Vary"
  normal
  100 25 do i font-size!
      ."      Chars per line: " i . font-cr 5 +loop
;

( ------------------------------------------------------------ )

: aspect-can-vary
  .title" Aspect Ratio Can Vary"
  normal
  180 20 do normal font-width @ i 100 */ font-width !
      ."    Percent normal: " i . bspace ." %" font-cr 15 +loop
;

( ------------------------------------------------------------ )

: weight-can-vary
  .title" Weight Can Vary"
  normal
  10 0 do i 1+ dup 30 font-pick
      ."     Weight " . font-cr loop
;

( ------------------------------------------------------------ )

: slant-can-vary
  .title" Slant Can Vary"
  normal
  130 -120 do i font-slant !
      ."         Slant percent: "
      i . bspace ." %" font-cr 15 +loop
;

( ------------------------------------------------------------ )

: applications
  .title" Applications"
  *f" Slide shows"
  *f" Intercept gforth console I/O"
;

( ------------------------------------------------------------ )

: how-big
  .title" How big is it?"
  *f" Source in 64 char blockish lines:"
  +f"   156 font.fs"
  +f"    95 gimple1.fs"
  +f"   206 grf.fs"
  +f"   393 slides.fs"
  +f"    30 terminal.fs"
  +f"    88 xlib.fs"
  +f"   968 total"
  *f" ~5k of core code"
  *f" ~512 bytes of font data"
;

( ------------------------------------------------------------ )

: future
  .title" Future"
  *f" Huffman encode points?"
  *f" Support color"
  *f" Refactor"
  *f" Gimple2"
;

( ------------------------------------------------------------ )

: questions?
  .title" Questions?"
  font-cr font-cr
  2 50 font-pick
  ."   Code online at:" font-cr
  ."     https://githib.com/bradn123/simplefont" font-cr
;

( ------------------------------------------------------------ )
( Slide deck )

create slides
' title ,
' motivation ,
' ground-rules ,
' simple-graphics ,
' simple-events ,
' bezier-curves ,
' de-castelijau-subdivision ,
' quartic-curves-in-forth ,
' quartic-drawing ,
' anti-aliased-pen ,
' font-layout ,
' font-format ,
' memory-format ,
' gimple1-slide ,
' size-can-vary ,
' aspect-can-vary ,
' weight-can-vary ,
' slant-can-vary ,
' applications ,
' how-big ,
' future ,
' questions? ,
here slides - cell / constant slide-count


( ------------------------------------------------------------ )
( Slide drawing )

variable slide
: slide-num   tiny home height font-margin @ - font-y !
              slide @ 1+ . ." of " slide-count . ;
: draw   slides slide @ cells + @ clear execute slide-num flip ;
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
