import { motion } from "framer-motion";
import React from "react";

let base = 4;
const t = (d: number) => d * base;

const AnimateTableRowItem = ({ children }: React.PropsWithChildren) => (
    <motion.li
        className='relative py-3 border-b border-gray-800'
        initial={{height: 0, opacity: 0}}
        animate={{
            height: 'auto',
            opacity: 1,
            transition: {type: 'spring', bounce: 0.3, opacity: {delay: t(0.025)}}
        }}
        exit={{height: 0, opacity: 0}}
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
    </motion.li>
)

export default AnimateTableRowItem;