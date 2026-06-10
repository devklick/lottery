import { Button, Flex, Stack } from "@mantine/core";
import TimedOverlay from "../../components/TimedOverlay";
import { useEffect, useState } from "react";
import NumberBall from "../../components/NumberBall/NumberBall";

interface SelectionPickerProps {
  selectionNumbers: ReadonlyArray<number>;
  selectedNumbers: ReadonlyArray<number>;
  requiredCount: number;
  onSubmit(selectionNumbers: Array<number>): void;
  submitStatus: "waiting" | "submitting" | "success";
  onDone(): void;
}

function SelectionPicker({
  selectionNumbers,
  selectedNumbers: _selectedNumbers,
  requiredCount,
  onSubmit,
  submitStatus,
  onDone,
}: SelectionPickerProps) {
  const [showOverlay, setShowOverlay] = useState(false);
  const [selectedNumbers, setSelectedNumbers] = useState([..._selectedNumbers]);

  useEffect(() => {
    setSelectedNumbers([..._selectedNumbers]);
  }, [_selectedNumbers]);

  useEffect(() => {
    if (submitStatus === "success") setShowOverlay(true);
  }, [submitStatus]);

  function handleSelected(selectionNumber: number) {
    if (selectedNumbers.includes(selectionNumber)) {
      setSelectedNumbers((cur) => cur.filter((c) => c !== selectionNumber));
    } else if (selectedNumbers.length < requiredCount) {
      setSelectedNumbers((cur) => [...cur, selectionNumber]);
    }
  }

  function handleOverlayFinished() {
    setShowOverlay(false);
    onDone();
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
        <TimedOverlay
          duration={3000}
          onTimeElapsed={handleOverlayFinished}
          show={showOverlay}
        />
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
