import React from "react";
import { useController } from "react-hook-form";

interface CustomElementInputProps {
  element: React.ReactNode;
  name: string;
}

function CustomElementInput({ name, element }: CustomElementInputProps) {
  const {
    field,
    fieldState: { error: fieldError },
    formState: { defaultValues },
  } = useController({ name });
  return element;
}

export default CustomElementInput;
