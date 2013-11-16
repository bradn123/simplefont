s" grf.fs" included
s" font.fs" included
s" gimple1.fs" included
( ------------------------------------------------------------ )


( ------------------------------------------------------------ )
( Formatting utilities )

: centered ( n -- ) width swap font-width @ * - 2/ font-x ! ;
: center-type dup centered font-type font-cr ;

( ------------------------------------------------------------ )

: title-slide
  s" Testing centered" center-type
  s" The quick brown fox jumped over the lazy dog." font-type
  font-cr
;

( ------------------------------------------------------------ )

: slide1
  s" Hello there" center-type
  s" The quick brown fox jumped over the lazy dog." font-type
  font-cr
;

( ------------------------------------------------------------ )
( Slide deck )

create slides
' title-slide ,
' slide1 ,
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
  1024 768 window
  begin
    wait
    event case
      expose-event of draw endof
      resize-event of draw endof
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
