import {
  Badge,
  BadgeProps,
  Button,
  Flex,
  Stack,
  Text,
  useComputedColorScheme,
  useMantineTheme,
} from "@mantine/core";
import TimedOverlay from "../../components/TimedOverlay";
import { useEffect, useState } from "react";

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
  const theme = useMantineTheme();
  const colorScheme = useComputedColorScheme();
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

  function getSelectionStyle(selectionNumber: number): BadgeProps {
    const isSelected = selectedNumbers.includes(selectionNumber);
    const allSelected = selectedNumbers.length === requiredCount;

    return {
      circle: true,
      size: "xl",
      color: isSelected
        ? theme.colors.green[colorScheme === "light" ? 6 : 8]
        : allSelected
          ? theme.colors.gray[colorScheme === "light" ? 2 : 8]
          : "gray",
      style: {
        cursor: allSelected ? "not-allowed" : "pointer",
      },
    };
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
          .map((selection) => (
            <Badge
              component="button"
              key={selection}
              {...getSelectionStyle(selection)}
              onClick={() => handleSelected(selection)}
            >
              {selection}
            </Badge>
          ))}
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
