TARGET_DLL = bin/Slerpy.dll
TARGET_DEF = bin/Slerpy.def
TARGET_LIB = bin/Slerpy.a

OBJS = obj/Types.o obj/Transformation.o

all: directories $(TARGET_DLL)

clean:
	rm -f $(OBJS) $(TARGET_DLL) $(TARGET_DEF) $(TARGET_LIB)

rebuild: clean all

directories: obj/ bin/

obj/:
	mkdir obj/

bin/:
	mkdir bin/

$(TARGET_DLL): $(OBJS)
	g++ --shared -g -o $@ $^ -Wl,--output-def,$(TARGET_DEF),--out-implib,$(TARGET_LIB)

obj/%.o: %.cpp %.h
	g++ -D_WIN32 -D_SLERPYCORE_EXPORT -g -MMD -MP -o $@ -c $<

-include $(OBJS:.o=.d)
