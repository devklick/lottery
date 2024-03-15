import { useState } from "react";
import { MultiSelect as _MultiSelect } from "@mantine/core";
import { useController } from "react-hook-form";
import { MultiSelectProps, WithName } from "../types";
import ErrorMessage from "./ErrorMessage";

function MultiSelect(props: WithName<MultiSelectProps>) {
  const { label, name, ...rest } = props;
  // const [options, setOptions] = useState(props.options);
  const {
    field,
    fieldState: { error: fieldError },
    formState: { defaultValues },
  } = useController({ name });

  const error = fieldError ? (
    <ErrorMessage>{fieldError.message?.toString()}</ErrorMessage>
  ) : undefined;

  const { onChange, ...restField } = field;

  return (
    <_MultiSelect
      id={name}
      label={label}
      data={props.options}
      onChange={(value) => {
        onChange(value ?? defaultValues?.[value]);
      }}
      error={error}
      // creatable
      // getCreateLabel={(query) => `+ ${query}`}
      // onCreate={(query) => {
      //   const capitalized = query.charAt(0).toUpperCase() + query.substring(1);
      //   const item = { label: capitalized, value: query };
      //   setOptions((prev) => [...prev, item]);
      //   return item;
      // }}
      {...rest}
      {...restField}
    />
  );
}

export default MultiSelect;
