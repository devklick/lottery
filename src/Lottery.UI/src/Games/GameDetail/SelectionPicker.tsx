import { Button, Flex, Stack } from "@mantine/core";
import { useEffect, useState } from "react";

import NumberBall from "../../components/NumberBall/NumberBall";

interface SelectionPickerProps {
  selectionNumbers: ReadonlyArray<number>;
  selectedNumbers: ReadonlyArray<number>;
  requiredCount: number;
  onSubmit(selectionNumbers: Array<number>): void;
  submitStatus: "waiting" | "submitting" | "success";
}

function SelectionPicker({
  selectionNumbers,
  selectedNumbers: _selectedNumbers,
  requiredCount,
  onSubmit,
  submitStatus,
}: SelectionPickerProps) {
  const [selectedNumbers, setSelectedNumbers] = useState([..._selectedNumbers]);

  useEffect(() => {
    setSelectedNumbers([..._selectedNumbers]);
  }, [_selectedNumbers]);

  function handleSelected(selectionNumber: number) {
    if (selectedNumbers.includes(selectionNumber)) {
      setSelectedNumbers((cur) => cur.filter((c) => c !== selectionNumber));
    } else if (selectedNumbers.length < requiredCount) {
      setSelectedNumbers((cur) => [...cur, selectionNumber]);
    }
  }

  const remaining = requiredCount - selectedNumbers.length;

  const submitButtonText = (() => {
    if (submitStatus === "submitting") return "Submitting";
    if (remaining) return `Submit (${remaining}/${requiredCount} remaining)`;
    return "Submit";
  })();

  return (
    <Stack align="center">
      <Flex
        gap={"lg"}
        align={"center"}
        justify={"center"}
        maw={500}
        wrap={"wrap"}
        style={{ position: "relative" }}
        p={10}
      >
        {[...selectionNumbers]
          ?.sort((a, b) => a - b)
          .map((selection) => {
            const isSelected = selectedNumbers.includes(selection);
            const disabled =
              selectedNumbers.length === requiredCount && !isSelected;
            return (
              <NumberBall
                selected={isSelected}
                disabled={disabled}
                value={selection}
                onClick={handleSelected}
              />
            );
          })}
      </Flex>
      <Button
        onClick={() => onSubmit(selectedNumbers)}
        disabled={selectedNumbers.length !== requiredCount}
      >
        {submitButtonText}
      </Button>
    </Stack>
  );
}

export default SelectionPicker;
