import {useState, useRef, useEffect} from "react";
import {GetAccessToken} from "../../services/StorageService.ts";

type PredictiveTextProps = {
    placeholder?: string,
    value?: string,
    onChange?: (value: string) => void,
}

const PredictiveText = (props: PredictiveTextProps) => {
    const [userText, setUserText] = useState(props.value ?? "");
    const [aiText, setAIText] = useState(!props.value ? (props.placeholder ?? "") : "");
    const [loading, setLoading] = useState(false);

    const debounceTimeoutRef = useRef<any|null>(null);
    const contentEditableRef = useRef<HTMLSpanElement|null>(null);
    
    const url = `${import.meta.env.VITE_API_BASE_PATH}/textCompletion/api/textCompletion`;

    let enterPressed = false;

    useEffect(() => {
        if (props.onChange) {
            props.onChange(userText);
        }
    }, [userText, props.onChange]);

    useEffect(() => {
        if (props.value) {
            contentEditableRef.current!.textContent = props.value;
        }
    }, [contentEditableRef]);

    const fetchSuggestions = (text: string) => {
        if (text.trim().length) {
            setLoading(true);
            fetch(url, {
                    method: "POST",
                    body: JSON.stringify({ input: text }),
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${GetAccessToken()}`,
                    }
                }
            )
                .then((res) => res.json())
                .then((data) => {
                    setAIText(data.text); // Update this line to match your JSON structure
                    setLoading(false);
                })
                .catch((error) => {
                    console.error("Error fetching AI text:", error);
                    setLoading(false);
                });
        }
    };

    const isCursorAtEnd = () => {
        const selection = window.getSelection();
        return selection?.anchorOffset === selection?.anchorNode?.length;
    };

    const handleInput = (e: any) => {
        let newText = e.target.innerText;
        if (enterPressed && newText.endsWith("\n\n")) {
            // Remove the last newline character
            newText = newText.slice(0, -1);

            // Reset the flag
            enterPressed = false;
        }

        setUserText(newText);
        setAIText("");

        // Check if cursor is at the end
        if (isCursorAtEnd()) {
            // Debounce the API call
            clearTimeout(debounceTimeoutRef.current);
            debounceTimeoutRef.current = setTimeout(() => {
                fetchSuggestions(newText);
            }, 1500);
        }
    };

    const focusContentEditable = () => {
        if (contentEditableRef.current) {
            contentEditableRef.current.focus();
        }
    };

    const setCursorToEnd = (element: any) => {
        const range = document.createRange();
        const selection = window.getSelection();
        
        range.selectNodeContents(element);
        range.collapse(false); // false means collapse to end rather than the start
        
        selection?.removeAllRanges();
        selection?.addRange(range);
    };

    const acceptSuggestion = () => {
        const contentEditableElement = contentEditableRef.current!;
        
        if (aiText) {
            setUserText(userText + aiText);
            contentEditableElement.innerText = userText + aiText;
            setAIText("");
            setCursorToEnd(contentEditableElement);
        }
    };

    const handleKeyDown = (event: any) => {
        if (event.key === "Tab") {
            event.preventDefault();
            acceptSuggestion();
        }

        if (event.key === "Enter") {
            // Set the flag to true when Enter is pressed
            enterPressed = true;

            // Allow the default Enter key behavior to occur
            setTimeout(() => {
                const contentEditableElement = contentEditableRef.current;
                const childNodes = Array.from(contentEditableElement!.childNodes);

                // Find the last <br> element
                for (let i = childNodes.length - 1; i >= 0; i--) {
                    if (childNodes[i].nodeName === "BR") {
                        // Remove the last <br> element
                        contentEditableElement!.removeChild(childNodes[i]);
                        break; // Exit the loop after removing the <br>
                    }
                }

                // Insert an empty text node with a zero-width space
                const emptyTextNode = document.createTextNode("\u200B");
                contentEditableElement!.appendChild(emptyTextNode);

                // Set cursor after the empty text node
                setCursorToEnd(contentEditableElement);
            }, 0); // SetTimeout with delay of 0 to allow the stack to clear and the <br> to be inserted
        }
    };

    return (
        <div className="flex flex-col place-items-center place-content-center relative">
            {
                loading && <span className='absolute top-2 right-2'>
                    <span className='loading loading-sm loading-spinner' />
                </span>
            }
            <div
                onClick={focusContentEditable}
                className="p-4 input focus:outline-0 shadow cursor-text rounded-lg text-left w-full min-h-[100px] mx-auto overflow-auto"
            >
                <span
                    ref={contentEditableRef}
                    className="border-0 outline-none"
                    contentEditable={true}
                    suppressContentEditableWarning={true}
                    onInput={handleInput}
                    onKeyDown={handleKeyDown}
                >
                     {/*{userText} */}
                </span>

                <span
                    className={`text-gray-600 transition-opacity duration-500 ${aiText ? "opacity-100" : "opacity-0"
                    }`}
                    contentEditable={false}
                >
                    {aiText.length > 0 && (
                        <>
                            {aiText}
                            <span
                                onClick={() => {
                                    acceptSuggestion();
                                }}
                                className="border p-1.5 py-0.5 text-[10px] ml-1 inline-block w-fit rounded-md border-gray-300 cursor-pointer"
                            >
                                Tab
                            </span>
                        </>
                    )}
                </span>
            </div>
        </div>
    );
}

export default PredictiveText