import { Flex, InputLabelProps, Skeleton, StyleProp } from "@mantine/core";
import { CSSProperties, PropsWithChildren } from "react";

import { useWaitFor } from "../../common/hooks/time.hooks";
import FieldLabel from "../FieldLabel";

interface FieldWrapperProps {
  name?: string;
  description?: string;
  required?: boolean;
  loading?: boolean;
  minLoadingTime?: number;
  direction?: StyleProp<CSSProperties["flexDirection"]>;
  fieldFlex?: StyleProp<CSSProperties["flex"]>;
  labelProps?: InputLabelProps;
}

export default function FieldWrapper({
  name,
  description,
  children,
  required,
  loading,
  minLoadingTime = 400,
  direction = "column",
  fieldFlex,
  labelProps,
}: PropsWithChildren<FieldWrapperProps>) {
  const showSkeleton = useWaitFor(loading ?? false, minLoadingTime);

  const skeleton = (
    <Skeleton visible={showSkeleton} flex={fieldFlex}>
      {children}
    </Skeleton>
  );

  if (!name) return skeleton;

  return (
    <Flex gap={"xs"} direction={direction} w="100%" className="field-wrapper">
      <FieldLabel
        name={name}
        required={required}
        description={description}
        loading={showSkeleton}
        labelProps={labelProps}
      />
      {skeleton}
    </Flex>
  );
}
