import {domAnimation, LazyMotion, m} from "framer-motion";
import React from "react";

let base = 4;
const t = (d: number) => d * base;

const AnimateTableRowItem = ({ className, children }: React.PropsWithChildren<{ className?: string }>) => (
    <LazyMotion features={domAnimation} strict>
        <m.li
            className={`relative py-3 border-b border-gray-800 ${className??''}`}
            initial={{scaleY: 0, opacity: 0}}
            animate={{
                scaleY: 1,
                opacity: 1,
                transition: {type: 'spring', bounce: 0.3, opacity: {delay: t(0.025)}}
            }}
            exit={{scaleY: 0, opacity: 0}}
            transition={
                {
                    duration: t(0.15),
                    type: 'spring',
                    bounce: 0,
                    opacity: {duration: t(0.03)},
                }
            }
        >
            {children}
        </m.li>
    </LazyMotion>
)

export default AnimateTableRowItem;